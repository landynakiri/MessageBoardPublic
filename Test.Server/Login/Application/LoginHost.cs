using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Protocol;
using Test.Common;
using Test.Common.TaskSchedulers;
using Test.Common.Tcp.ConnectObserver;
using Test.Common.Tcp.ServicePeer;
using Test.Server.Extension;
using Test.Server.Login.Domain;
using Test.Server.SQLService;
using Test.Server.SQLService.Table;

namespace Test.Server.Login.Application
{
	public sealed class LoginHost : IHostedService, IObserver<ConnectInfo>
	{
		private readonly ILogger logger;
		private readonly IServicePeerReceive servicePeerReceive;
		private readonly IServicePeerSend servicePeerSend;
		private readonly DBConnector dBConnection;
		private readonly LoginInfo loginInfo;
		private readonly IObservable<ConnectInfo> connectStatusObservable;
		private readonly IScheduler scheduler;
		private IDisposable disconnectSubscriber;

		public LoginHost(
			IServicePeerReceive servicePeerReceive,
			IServicePeerSend servicePeerSend,
			DBConnector dBConnection,
			LoginInfo loginInfo,
			IObservable<ConnectInfo> connectStatusObservable,
			IScheduler scheduler,
			ILogger<LoginHost> logger)
		{
			this.servicePeerReceive = servicePeerReceive;
			this.servicePeerSend = servicePeerSend;
			this.dBConnection = dBConnection;
			this.loginInfo = loginInfo;
			this.connectStatusObservable = connectStatusObservable;
			this.scheduler = scheduler;
			this.logger = logger;
			this.servicePeerReceive.AddCommandHandler((short)ServiceCommand.Login, OnLoginReq);
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			logger.LogInformation("Start");

			disconnectSubscriber = connectStatusObservable.Subscribe(this);

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			logger.LogInformation("Stop");

			disconnectSubscriber.Dispose();

			return Task.CompletedTask;
		}

		private void OnLoginReq(int serialNumber, byte[] protoData)
		{
			scheduler.Enqueue(() =>
			{
				LoginReq login = ProtocolBuffConvert.Deserialize<LoginReq>(protoData.ToArray());

				LoginResult loginResult = LoginResult.Failed;

				if (string.IsNullOrEmpty(login.Account) && string.IsNullOrEmpty(login.Password))
				{
					loginResult = LoginResult.AccountOrPasswordCannotEmpty;
				}
				else
				{
					Account account = dBConnection.FindDataAsync<Account>(new DBConnectInfo("account"), new object[] { login.Account }).GetAwaiter().GetResult();
					if (account == null)
					{
						account = new Account()
						{
							ID = login.Account,
							Password = login.Password,
						};

						if (dBConnection.InsertPrimaryKeyDataAsync<Account, string>(new DBConnectInfo("account"), account).GetAwaiter().GetResult())
						{
							logger.LogInformation($"Create User {login.Account}.");
							logger.LogInformation($"User {login.Account} Login.");

							loginResult = LoginResult.Ok;
						}
					}
					else
					{
						logger.LogInformation($"User {login.Account} Login.");

						loginResult = LoginResult.Ok;
					}
				}

				if (loginResult == LoginResult.Ok)
				{
					loginInfo.AddPlayer(serialNumber, new Player() { Name = login.Account });
				}

				servicePeerSend.SendMessageAsync(serialNumber, (short)ServiceCommand.Login, new LoginResp()
				{
					Name = login.Account,
					LoginResult = loginResult,
				}).GetAwaiter().GetResult();
			});
		}

		void IObserver<ConnectInfo>.OnNext(ConnectInfo value)
		{
			if (value.ConnectState == ConnectState.Disconnect)
			{
				loginInfo.RemovePlayer(value.SerialNumber);
			}
		}

		void IObserver<ConnectInfo>.OnError(Exception error)
		{
			// Do nothing.
		}

		void IObserver<ConnectInfo>.OnCompleted()
		{

		}
	}
}
