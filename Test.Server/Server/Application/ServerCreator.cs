using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Test.Common.Tcp.ConnectObserver;
using Test.Common.Tcp.ServicePeer;

namespace Test.Server.Server.Application
{
	public class ServerCreator
	{
		private readonly ConnectorConsole connectorConsole;
		private readonly ConnectStatusMonitor connectStatusMonitor;
		private IServicePeerReceive servicePeer;
		private readonly ILogger<TcpServer> logger;

		private HashSet<TcpServer> tcpServers = new HashSet<TcpServer>();

		public ServerCreator(ConnectorConsole connectorConsole, ConnectStatusMonitor connectStatusMonitor, IServicePeerReceive servicePeer, ILogger<TcpServer> logger)
		{
			this.connectorConsole = connectorConsole;
			this.connectStatusMonitor = connectStatusMonitor;
			this.servicePeer = servicePeer;
			this.logger = logger;
		}

		internal async Task StartUpAsync(int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				TcpServer tcpServer = new TcpServer(i, GlobalSetting.Port + i, connectorConsole, connectStatusMonitor, servicePeer, logger);
				tcpServers.Add(tcpServer);
				_ = tcpServer.StartAsync();

				await Task.Yield();
			}
		}
	}
}
