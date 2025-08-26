using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Test.Common.TaskSchedulers;
using Test.Server.Redis;

namespace Test.Server.Server.Application
{
	public sealed class ServerHost : IHostedService
	{
		private readonly RedisConsole redisConsole;
		private readonly ServerCreator serverCreator;
		private readonly IScheduler taskScheduler;
		private readonly ILogger logger;

		public ServerHost(
			RedisConsole redisConsole,
			ServerCreator serverCreator,
			IScheduler taskScheduler,
			ILogger<ServerHost> logger,
			IHostApplicationLifetime appLifetime)
		{
			this.redisConsole = redisConsole;
			this.serverCreator = serverCreator;
			this.taskScheduler = taskScheduler;
			this.logger = logger;

			appLifetime.ApplicationStarted.Register(OnStarted);
			appLifetime.ApplicationStopping.Register(OnStopping);
			appLifetime.ApplicationStopped.Register(OnStopped);
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			logger.LogInformation("Start");

			await redisConsole.ConnectAsync();

			await serverCreator.StartUpAsync(GlobalSetting.ServerAmount);
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			logger.LogInformation("Stop");

			redisConsole.Disconnect();

			return Task.CompletedTask;
		}

		private void OnStarted()
		{
			logger.LogInformation("2. OnStarted has been called.");
		}

		private void OnStopping()
		{
			logger.LogInformation("3. OnStopping has been called.");
		}

		private void OnStopped()
		{
			logger.LogInformation("5. OnStopped has been called.");
		}
	}
}
