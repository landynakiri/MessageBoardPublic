using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using Test.Common.Tcp;
using Test.Common.Tcp.ConnectObserver;
using Test.Common.Tcp.ServicePeer;

namespace Test.Server.Server.Application
{
	public class Connector : TcpConsole
	{
		public Connector(IServicePeerReceive servicePeerReceive, ConnectStatusMonitor connectStatusMonitor, ILogger logger)
			: base(GlobalSetting.MininumBufferSize, servicePeerReceive, connectStatusMonitor, logger)
		{
		}
	}
}
