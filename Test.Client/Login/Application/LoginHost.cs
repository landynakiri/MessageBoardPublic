using System;
using System.Drawing;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Protocol;
using Test.Client.Chat.Domain;
using Test.Client.Client;
using Test.Client.Common.Application;
using Test.Client.Extension;
using Test.Client.Login.Domain;
using Test.Common;
using Test.Common.TaskSchedulers;
using Test.Common.Tcp.ConnectObserver;
using Test.Common.Tcp.ServicePeer;

namespace Test.Client.Login.Application
{
	public class LoginHost : IClientHostedService, ILogin, IObserver<ConnectInfo>
	{
		private readonly IUIController uiController;
		private readonly LoginInfo loginInfo;
		private readonly TcpConnector tcpConnector;
		private readonly IServicePeerReceive servicePeerReceive;
		private readonly IObservable<ConnectInfo> connectStatusObservable;
		private readonly Player player;
		private readonly MainInfo mainInfo;
		private readonly IScheduler scheduler;
		private IDisposable connectStatusSubscriber;
		private readonly ILogger<LoginHost> logger;

		public LoginHost(
			IUIController uiController,
			LoginInfo loginInfo,
			TcpConnector tcpConnector,
			IServicePeerReceive servicePeerReceive,
			IObservable<ConnectInfo> connectStatusObservable,
			Player player,
			MainInfo mainInfo,
			ILogger<LoginHost> logger)
		{
			this.uiController = uiController;
			this.loginInfo = loginInfo;
			this.tcpConnector = tcpConnector;
			this.servicePeerReceive = servicePeerReceive;
			this.connectStatusObservable = connectStatusObservable;
			this.player = player;
			this.mainInfo = mainInfo;
			this.logger = logger;
		}

		public void Start()
		{
			servicePeerReceive.AddCommandHandler((short)ServiceCommand.Login, OnLoginResp);

			uiController.RegisterEvent<ILogin>(UINames.MainForm, this);

			connectStatusSubscriber = connectStatusObservable.Subscribe(this);
		}

		public void Stop()
		{
			uiController.UnregisterEvent<ILogin>(UINames.MainForm, this);

			connectStatusSubscriber.Dispose();
		}

		void ILogin.OnClickLogin()
		{
			if (tcpConnector.IsConnected)
			{
				logger.LogInformation("不可重複登入");
				return;
			}

			tcpConnector.ConnectAsync().UndeadlockResult();

			LoginReq login = new LoginReq()
			{
				Account = loginInfo.Account,
				Password = loginInfo.Password,
			};
			tcpConnector.SendMessageAsync((short)ServiceCommand.Login, login).UndeadlockResult();

			mainInfo.SetConnectStatus(tcpConnector.IsConnected);
			mainInfo.SetOnlineStatus(player.IsOnline);
		}

		private void OnLoginResp(int serialNumber, byte[] protoData)
		{
			LoginResp loginResp = ProtocolBuffConvert.Deserialize<LoginResp>(protoData);

			player.IsOnline = loginResp.LoginResult == LoginResult.Ok;
			player.Name = loginResp.Name;

			mainInfo.SetOnlineStatus(player.IsOnline);

			logger.LogInformation($"Login {loginResp.LoginResult}, Name is {loginResp.Name}");
		}

		void ILogin.OnClickLogout()
		{
			tcpConnector.Disconnect();
		}

		void IObserver<ConnectInfo>.OnNext(ConnectInfo value)
		{
			if (!tcpConnector.IsConnected)
			{
				player.IsOnline = false;
				player.Name = string.Empty;
			}

			mainInfo.SetConnectStatus(tcpConnector.IsConnected);

			mainInfo.SetOnlineStatus(player.IsOnline);

			logger.LogInformation($"Connect State {value.ConnectState}");
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
