using Test.Common.Observer;

namespace Test.Common.Tcp.ConnectObserver
{
	public class ConnectStatusMonitor : BaseObserver<ConnectInfo>
	{
		public void Invoke(ConnectInfo connectInfo)
		{
			foreach (var observer in Observers)
			{
				observer.OnNext(connectInfo);
			}

			foreach (var observer in Observers.ToArray())
			{
				if (observer != null)
				{
					observer.OnCompleted();
				}
			}
		}
	}
}
