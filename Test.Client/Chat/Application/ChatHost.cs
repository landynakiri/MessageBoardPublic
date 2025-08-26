using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Protocol;
using Test.Client.Chat.Domain;
using Test.Client.Client;
using Test.Client.Common.Application;
using Test.Client.Extension;
using Test.Client.Login.Application;
using Test.Common;
using Test.Common.Tcp.ServicePeer;

namespace Test.Client.Chat.Application
{
	public class ChatHost : IClientHostedService, IChatSend
	{
		private readonly TcpConnector tcpConnector;
		private readonly MainInfo mainInfo;
		private readonly IServicePeerReceive servicePeerReceive;
		private readonly ILogger<ChatHost> logger;
		private IUIController uiController;

		public ChatHost(
			TcpConnector tcpConnector,
			MainInfo mainInfo,
			IServicePeerReceive servicePeerReceive,
			IUIController uiController,
			ILogger<ChatHost> logger)
		{
			this.tcpConnector = tcpConnector;
			this.mainInfo = mainInfo;
			this.servicePeerReceive = servicePeerReceive;
			this.uiController = uiController;
			this.logger = logger;
		}

		public void Start()
		{
			servicePeerReceive.AddCommandHandler((short)ServiceCommand.SendMessage, OnSendMessageResp);

			uiController.RegisterEvent<IChatSend>(UINames.MainForm, this);
		}

		public void Stop()
		{
			uiController.UnregisterEvent<IChatSend>(UINames.MainForm, this);
		}

		void IChatSend.OnClickSendMessage()
		{
			SendMessageReq sendMessageReq = new SendMessageReq()
			{
				Text = mainInfo.SendMessage,
			};

			tcpConnector.SendMessageAsync((short)ServiceCommand.SendMessage, sendMessageReq).UndeadlockResult();

			mainInfo.SendMessage = string.Empty;
		}

		private void OnSendMessageResp(int serialNumber, byte[] protoData)
		{
			SendMessageResp resp = ProtocolBuffConvert.Deserialize<SendMessageResp>(protoData);

			if (resp.Result == SendMessageResult.Ok)
			{
				mainInfo.AddChatCommit(resp.Name, resp.Text);
				logger.LogInformation($"ChatCommit {resp.Text}");
			}
			else
			{
				logger.LogInformation($"SendMessageResult {resp.Result}");
			}
		}
	}
}
