using System.Collections.Generic;
using Test.Client.Chat.Application;

namespace Test.Client.Login.Application
{
	public class StartUpClient : IClientHostedService
	{
		private List<IClientHostedService> hosts = new List<IClientHostedService>();

		public StartUpClient(
			LoginHost loginHost,
			ChatHost chatHost)
		{
			hosts.Add(loginHost);
			hosts.Add(chatHost);
		}

		public void Start()
		{
			StartAllHosts();
		}

		private void StartAllHosts()
		{
			for (int i = 0; i < hosts.Count; i++)
			{
				hosts[i].Start();
			}
		}

		public void Stop()
		{
			StopAllHosts();
		}

		private void StopAllHosts()
		{
			for (int i = 0; i < hosts.Count; i++)
			{
				hosts[i].Stop();
			}
		}
	}
}
