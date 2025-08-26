using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Test.Common.Tcp.ConnectObserver;
using Test.Common.Tcp.ServicePeer;

namespace Test.Common.Tcp
{
	public class TcpConsole : IDisposable
	{
		private readonly int mininumBufferSize;
		protected TcpClient TcpClient;
		private readonly IServicePeerReceive servicePeerReceive;
		protected readonly ConnectStatusMonitor ConnectStatusMonitor;
		protected readonly ILogger Logger;
		private Pipe pipe;
		private PipeWriter writer;
		private PipeReader reader;
		private byte[] peekByte = new byte[1];

		public bool IsConnected => TcpClient is null ? false : TcpClient.Connected;

		public ConnectInfo ConnectInfo = new ConnectInfo();

		public TcpConsole(int mininumBufferSize, IServicePeerReceive servicePeerReceive, ConnectStatusMonitor connectStatusMonitor, ILogger logger)
		{
			this.mininumBufferSize = mininumBufferSize;
			this.servicePeerReceive = servicePeerReceive;
			ConnectStatusMonitor = connectStatusMonitor;
			Logger = logger;
		}

		public void Init(TcpClient tcpClient)
		{
			this.TcpClient = tcpClient;
			pipe = new Pipe();
			writer = pipe.Writer;
			reader = pipe.Reader;
		}

		public void Dispose()
		{
			TcpClient.Close();
			TcpClient.Dispose();
			TcpClient = null;

			writer.Complete();
			reader.Complete();
		}

		public async Task StartPeekAsync(int peekTimeMs)
		{
			try
			{
				while (TcpClient != null && TcpClient.Connected)
				{
					//使用Peek測試連線是否仍存在
					if (TcpClient.Client.Poll(0, SelectMode.SelectRead) && TcpClient.Client.Receive(peekByte, SocketFlags.Peek) == 0)
					{
						Dispose();
						InvokeDisconnect();
					}

					await Task.Delay(peekTimeMs);
				}
			}
			catch (SocketException ex)
			{
				Dispose();
				Logger.LogInformation(ex.ToString());
				InvokeDisconnect();
			}
		}

		public async Task StartReceiveAsync()
		{
			await ReceiveAsync();
		}

		private async Task ReceiveAsync()
		{
			NetworkStream stream = TcpClient.GetStream();

			Logger.LogInformation($"[{TcpClient}]Connector Start Receive...");

			try
			{
				while (true)
				{
					Memory<byte> memory = writer.GetMemory(mininumBufferSize);
					byte[] buffer = memory.ToArray();
					try
					{
						int received = await stream.ReadAsync(buffer, 0, buffer.Length);
						if (received == 0)
						{
							return;
						}

						Logger.LogInformation($"Recevie Message, Length is {received}");

						ArraySegment<byte> myArrSegAll = new ArraySegment<byte>(buffer, 0, received);
						var flushResult = await writer.WriteAsync(myArrSegAll);
					}
					catch (Exception ex)
					{
						TcpClient?.Close();
						TcpClient?.Dispose();
						TcpClient = null;

						Logger.LogInformation(ex.ToString());
						InvokeDisconnect();
						break;
					}

					// Make the data available to the PipeReader.
					FlushResult result = await writer.FlushAsync();

					if (result.IsCompleted)
					{
						return;
					}
				}
			}
			finally
			{
				// By completing PipeWriter, tell the PipeReader that there's no more data coming.
				await writer.CompleteAsync();
			}
		}

		public async Task StartReadAsync()
		{
			await ReadAsync();
		}

		private async Task ReadAsync()
		{
			Logger.LogInformation("Connector Start Read...");

			try
			{
				while (true)
				{
					// 讀取管子內目前的資料狀況
					var result = await reader.ReadAsync();

					var buffer = result.Buffer;

					try
					{
						if (TryReadBuffer(ref buffer, out ReadOnlySequence<byte> data))
						{
							ProcessData(data);
						}

						// Stop reading if there's no more data coming.
						if (result.IsCompleted)
						{
							if (buffer.Length > 0)
							{
								// The message is incomplete and there's no more data to process.
								throw new InvalidDataException("Incomplete message.");
							}

							break;
						}
					}
					finally
					{
						// Since all messages in the buffer are being processed, you can use the
						// remaining buffer's Start and End position to determine consumed and examined.
						reader.AdvanceTo(buffer.Start, buffer.End);
					}
				}
			}
			finally
			{
				// Mark the PipeReader as complete.
				await reader.CompleteAsync();
			}
		}

		private bool TryReadBuffer(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> data)
		{
			if (buffer.Length <= 0)
			{
				data = default;
				return false;
			}

			data = buffer.Slice(buffer.Start, buffer.End);
			buffer = buffer.Slice(buffer.GetPosition(data.Length, buffer.Start));
			return true;
		}

		private void ProcessData(ReadOnlySequence<byte> readOnlySequence)
		{
			if (readOnlySequence.Length <= 0)
			{
				return;
			}

			ReadOnlySequence<byte> commandRos = readOnlySequence.Slice(0, ServiceSetting.CommandLength);
			ReadOnlySequence<byte> protoData = readOnlySequence.Slice(ServiceSetting.CommandLength, readOnlySequence.End);

			short command = BitConverter.ToInt16(commandRos.ToArray(), 0);
			servicePeerReceive.OnReceiveCommand(GetHashCode(), command, protoData.ToArray());
		}

		private void InvokeDisconnect()
		{
			Logger.LogInformation($"Connector {GetHashCode()} Disconnected");
			ConnectInfo.SerialNumber = GetHashCode();
			ConnectInfo.ConnectState = ConnectState.Disconnect;
			ConnectStatusMonitor.Invoke(ConnectInfo);
		}

		public async Task SendMessageAsync(short command, byte[] message)
		{
			if (TcpClient is null || !TcpClient.Connected)
			{
				return;
			}

			NetworkStream stream = TcpClient.GetStream();
			byte[] cd = BitConverter.GetBytes(command);
			byte[] buffer = new byte[cd.Length + message.Length];
			Buffer.BlockCopy(cd, 0, buffer, 0, cd.Length);
			Buffer.BlockCopy(message, 0, buffer, cd.Length, message.Length);

			await stream.WriteAsync(buffer, 0, buffer.Length);
		}
	}
}
