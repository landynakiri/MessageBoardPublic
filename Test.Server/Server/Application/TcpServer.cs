using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Test.Common.Tcp.ConnectObserver;
using Test.Common.Tcp.ServicePeer;

namespace Test.Server.Server.Application
{
	public class TcpServer
	{
		private readonly ILogger logger;
		private readonly IServicePeerReceive servicePeer;
		private readonly int serverID;
		private readonly int port;
		private ConnectorConsole connectorConsole;
		private readonly ConnectStatusMonitor connectStatusMonitor;

		public TcpServer(int serverID, int port, ConnectorConsole connectorConsole, ConnectStatusMonitor connectStatusMonitor, IServicePeerReceive servicePeer, ILogger<TcpServer> logger)
		{
			this.servicePeer = servicePeer;
			this.logger = logger;
			this.connectorConsole = connectorConsole;
			this.connectStatusMonitor = connectStatusMonitor;
			this.serverID = serverID;
			this.port = port;
		}

		public async Task StartAsync()
		{
			var ipEndPoint = new IPEndPoint(IPAddress.Any, port);
			TcpListener listener = new TcpListener(ipEndPoint);

			try
			{
				listener.Start();
				logger.LogInformation($"[{port}]Start Listen...");

				while (connectorConsole.ConnectAmount < GlobalSetting.MaxConnectAmount)
				{
					var tcpClient = await listener.AcceptTcpClientAsync();

					Connector connector = connectorConsole.AddNewConnector(tcpClient);

					_ = connector.StartReceiveAsync();
					_ = connector.StartReadAsync();
					_ = connector.StartPeekAsync(GlobalSetting.PeekTimeMs);

					logger.LogInformation($"[{port}]Client Connect Amount {connectorConsole.ConnectAmount}");

					connector.ConnectInfo.ConnectState = ConnectState.Online;
					connectStatusMonitor.Invoke(connector.ConnectInfo);
				}

				listener.Stop();

				logger.LogInformation($"[{port}]Max Connect Amount Stop Listen...");
			}
			finally
			{
				listener.Stop();
			}
		}
	}
}
