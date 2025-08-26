using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Test.Server.SQLService
{
	public class DBConnector
	{
		private readonly ILogger<DBConnector> logger;

		public DBConnector(ILogger<DBConnector> logger)
		{
			this.logger = logger;
		}

		public async Task<bool> InsertPrimaryKeyDataAsync<T, K>(DBConnectInfo dBConnectInfo, T data) where T : class, IPrimaryKeyContext<K>, new()
		{
			try
			{
				logger.LogInformation($"InsertPrimaryKeyDataAsync[{typeof(T).Name}] to {dBConnectInfo.Database}");
				using (var context = new PrimaryKeyDbContext<T, K>(new NpgsqlConnection(dBConnectInfo.GetConnectString())))
				{
					context.Data.Add(data);
					int result = await context.SaveChangesAsync();
					logger.LogInformation($"SaveChanges is {result}");
					return true;
				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
				return false;
			}
		}

		public async Task<bool> InsertDataAsync<T>(DBConnectInfo dBConnectInfo, T data) where T : class, new()
		{
			try
			{
				logger.LogInformation($"InsertData[{typeof(T).Name}] to {dBConnectInfo.Database}");
				using (var context = new CommonDbContext<T>(new NpgsqlConnection(dBConnectInfo.GetConnectString())))
				{
					context.Data.Add(data);
					int result = await context.SaveChangesAsync();
					logger.LogInformation($"SaveChanges is {result}");
					return true;
				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
				return false;
			}
		}

		public async Task<T> FindDataAsync<T>(DBConnectInfo dBConnectInfo, params object[] keyValues) where T : class, new()
		{
			try
			{
				logger.LogInformation($"Find[{typeof(T).Name}] from {dBConnectInfo.Database}");
				using (var context = new CommonDbContext<T>(new NpgsqlConnection(dBConnectInfo.GetConnectString())))
				{
					T entity = await context.Set<T>().FindAsync(keyValues);
					return entity;
				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex.ToString());
				return default;
			}
		}

		//public void UpdateData<T>(DBConnectInfo dBConnectInfo, string databaseName, T data) where T : CommonDbContext<T>, new()
		//{
		//	logger.LogInformation($"UpdateData[{typeof(T).Name}] to {databaseName}");
		//	using (var context = new CommonDbContext<T>(new NpgsqlConnection(dBConnectInfo.GetConnectString(databaseName))))
		//	{
		//		context.Data.Update(data);
		//		context.SaveChanges();
		//	}
		//}
	}
}
