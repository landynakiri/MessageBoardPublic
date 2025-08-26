using System.Threading.Tasks;

namespace Test.Common.Tcp.ServicePeer
{
	public interface IServicePeerSend
	{
		Task SendMessageAsync(int serialNumber, short command, byte[] message);
		Task SendMessageToAllAsync(short command, byte[] message);
	}
}
