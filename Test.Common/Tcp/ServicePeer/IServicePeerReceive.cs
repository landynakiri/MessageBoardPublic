namespace Test.Common.Tcp.ServicePeer
{
	public delegate void ServiceCommandHandler(int serialNumber, byte[] protoData);

	public interface IServicePeerReceive
	{
		void AddCommandHandler(short command, ServiceCommandHandler handler);
		void OnReceiveCommand(int serialNumber, short command, byte[] protoData);
	}
}