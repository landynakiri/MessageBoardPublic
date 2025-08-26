using System.Threading.Tasks;
using Google.Protobuf;
using Test.Common;
using Test.Common.Tcp.ServicePeer;

namespace Test.Server.Extension
{
	public static class ServicePeerExtension
	{
		public static async Task SendMessageAsync(this IServicePeerSend servicePeerSend, int serialNumber, short command, IMessage message)
		{
			await servicePeerSend.SendMessageAsync(serialNumber, command, ProtocolBuffConvert.Serialize(message));
		}

		public static async Task SendMessageToAllAsync(this IServicePeerSend servicePeerSend, short command, IMessage message)
		{
			await servicePeerSend.SendMessageToAllAsync(command, ProtocolBuffConvert.Serialize(message));
		}
	}
}
