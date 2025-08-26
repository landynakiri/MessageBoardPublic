using System;
using System.Collections.Concurrent;

namespace Test.Server.Login.Domain
{
	public class LoginInfo
	{
		private ConcurrentDictionary<int, Player> players = new ConcurrentDictionary<int, Player>();

		public bool AddPlayer(int serialNumber, Player player)
		{
			return players.TryAdd(serialNumber, player);
		}

		public bool RemovePlayer(int serialNumber)
		{
			return players.TryRemove(serialNumber, out _);
		}

		public (bool, string) GetPlayerName(int serialNumber)
		{
			if (players.TryGetValue(serialNumber, out Player player))
			{
				return (true, player.Name);
			}
			{
				return (false, default);
			}
		}
	}
}
