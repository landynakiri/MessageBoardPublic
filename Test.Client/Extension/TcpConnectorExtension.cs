using System.Threading.Tasks;
using Google.Protobuf;
using Test.Client.Client;
using Test.Common;

namespace Test.Client.Extension
{
	public static class TcpConnectorExtension
	{
		public static async Task SendMessageAsync(this TcpConnector tcpConnector, short command, IMessage message)
		{
			await tcpConnector.SendMessageAsync(command, ProtocolBuffConvert.Serialize(message));
		}
	}
}
