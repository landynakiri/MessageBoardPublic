using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Test.Common.Tcp;
using Test.Common.Tcp.ConnectObserver;
using Test.Common.Tcp.ServicePeer;

namespace Test.Client.Client
{
	public class TcpConnector : TcpConsole
	{
		private readonly string hostname;
		private readonly int port;

		public TcpConnector(string hostname, int port, IServicePeerReceive servicePeerReceive, ConnectStatusMonitor connectStatusMonitor, ILogger<TcpConnector> logger)
			: base(GlobalSetting.MininumBufferSize, servicePeerReceive, connectStatusMonitor, logger)
		{
			this.hostname = hostname;
			this.port = port;
		}

		public async Task ConnectAsync()
		{
			if (IsConnected)
			{
				Logger.LogInformation("已連線，請勿重複連線。");
				return;
			}

			Init(new TcpClient());

			Task task = TcpClient.ConnectAsync(hostname, port);

			Logger.LogInformation($"連線至{hostname}:{port}");

			if (await Task.WhenAny(task, Task.Delay(GlobalSetting.ConnectTimeOutMs)) == task && !task.IsFaulted)
			{
				_ = StartReceiveAsync();
				_ = StartReadAsync();

				ConnectInfo.ConnectState = ConnectState.Online;
				ConnectStatusMonitor.Invoke(ConnectInfo);
			}
			else
			{
				Logger.LogInformation("Task cancelled: Connect Time Out.");
			}
		}

		public void Disconnect()
		{
			Dispose();
		}
	}
}
