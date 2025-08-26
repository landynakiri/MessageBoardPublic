using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Test.Common.Tcp.ConnectObserver;
using Test.Common.Tcp.ServicePeer;

namespace Test.Server.Server.Application
{
	public class ConnectorConsole : IServicePeerSend, IObserver<ConnectInfo>
	{
		private readonly IServicePeerReceive servicePeerReceive;
		private readonly ConnectStatusMonitor connectStatusMonitor;
		private readonly IObservable<ConnectInfo> connectStatusObservable;
		private readonly ILogger logger;
		private readonly IDisposable connectStatusSubscribe;

		internal int ConnectAmount => connectors.Count;

		private ConcurrentDictionary<int, Connector> connectors = new ConcurrentDictionary<int, Connector>();

		public ConnectorConsole(
			IServicePeerReceive servicePeerReceive,
			ConnectStatusMonitor connectStatusMonitor,
			IObservable<ConnectInfo> connectStatusObservable,
			ILogger<ConnectorConsole> logger)
		{
			this.servicePeerReceive = servicePeerReceive;
			this.connectStatusMonitor = connectStatusMonitor;
			this.connectStatusObservable = connectStatusObservable;
			this.logger = logger;

			connectStatusSubscribe = connectStatusObservable.Subscribe(this);
		}

		~ConnectorConsole()
		{
			connectStatusSubscribe.Dispose();
		}

		internal Connector AddNewConnector(TcpClient tcpClient)
		{
			Connector connector = new Connector(servicePeerReceive, connectStatusMonitor, logger);
			connector.Init(tcpClient);
			if (connectors.TryAdd(connector.GetHashCode(), connector))
			{
				logger.LogInformation($"Add Connector {connector.GetHashCode()}.");
				return connector;
			}
			else
			{
				return null;
			}
		}

		public async Task SendMessageAsync(int serialNumber, short command, byte[] message)
		{
			if (connectors.TryGetValue(serialNumber, out Connector connector))
			{
				await connector.SendMessageAsync(command, message);
			}
			else
			{
				logger.LogError("Connector not found.");
			}
		}

		public async Task SendMessageToAllAsync(short command, byte[] message)
		{
			foreach (var con in connectors.Values)
			{
				await con.SendMessageAsync(command, message);
			}
		}

		public void OnNext(ConnectInfo value)
		{
			if (value.ConnectState == ConnectState.Disconnect)
			{
				connectors.TryRemove(value.SerialNumber, out _);
			}
		}

		public void OnError(Exception error)
		{

		}

		public void OnCompleted()
		{

		}
	}
}
