using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test.Common.Tcp.ServicePeer
{
	public class ServicePeerReceive : IServicePeerReceive
	{
		private Dictionary<short, ServiceCommandHandler> handlers = new Dictionary<short, ServiceCommandHandler>();

		public void AddCommandHandler(short command, ServiceCommandHandler handler)
		{
			handlers.Add(command, handler);
		}

		public void RemoveCommandHandler(short command)
		{
			handlers.Remove(command);
		}

		public void OnReceiveCommand(int serialNumber, short command, byte[] protoData)
		{
			if (handlers.TryGetValue(command, out ServiceCommandHandler handler))
			{
				handler?.Invoke(serialNumber, protoData);
			}
		}
	}
}
