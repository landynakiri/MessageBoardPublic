using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Test.Common.CustomLogger.Log4Net;
using Test.Common.TaskSchedulers;
using Test.Common.Tcp.ConnectObserver;
using Test.Common.Tcp.ServicePeer;
using Test.Server.Chat.Application;
using Test.Server.Common.Application;
using Test.Server.Login.Application;
using Test.Server.Login.Domain;
using Test.Server.Redis;
using Test.Server.Server.Application;
using Test.Server.SQLService;

namespace Test.Server
{
	internal class Program
	{
		static void Main(string[] args)
		{
			IHost host = Host.CreateDefaultBuilder(args)
				.ConfigureLogging((hostContext, logging) =>
				{
					logging.ClearProviders();
					logging.AddLog4NetLogger();
				})
				.ConfigureServices((hostContext, services) =>
				{
					services.AddSingleton<DBConnector>();

					RedisConsole redisConsole = new RedisConsole();
					services.AddSingleton<RedisConsole>(redisConsole);
					services.AddSingleton<INoSql>(redisConsole);

					IServicePeerReceive servicePeerReceive = new ServicePeerReceive();
					services.AddSingleton<IServicePeerReceive>(servicePeerReceive);
					ConnectStatusMonitor connectMonitor = new ConnectStatusMonitor();
					services.AddSingleton<ConnectStatusMonitor>(connectMonitor);
					services.AddSingleton<IObservable<ConnectInfo>>(connectMonitor);
					var logger = services.BuildServiceProvider().GetService<ILogger<ConnectorConsole>>();
					ConnectorConsole connectorConsole = new ConnectorConsole(servicePeerReceive, connectMonitor, connectMonitor, logger);
					services.AddSingleton<ConnectorConsole>(connectorConsole);
					services.AddSingleton<IServicePeerSend>(connectorConsole);
					services.AddSingleton<ServerCreator>();

					services.AddSingleton<IScheduler, TaskSchedulerConsole>();

					//Domain
					services.AddSingleton<LoginInfo>();

					//Application
					services.AddHostedService<ServerHost>();
					services.AddHostedService<LoginHost>();
					services.AddHostedService<ChatHost>();
				})
				.Build();

			host.Run();

			Console.ReadLine();
		}
	}
}
