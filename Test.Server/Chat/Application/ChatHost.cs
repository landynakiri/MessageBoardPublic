using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Protocol;
using Test.Common;
using Test.Common.TaskSchedulers;
using Test.Common.Tcp.ServicePeer;
using Test.Server.Chat.Domain;
using Test.Server.Common.Application;
using Test.Server.Extension;
using Test.Server.Login.Domain;

namespace Test.Server.Chat.Application
{
	public sealed class ChatHost : IHostedService
	{
		private readonly IServicePeerSend servicePeerSend;
		private readonly LoginInfo loginInfo;
		private readonly IScheduler scheduler;
		private readonly INoSql noSql;
		private readonly ILogger<ChatHost> logger;

		public ChatHost(
			IServicePeerReceive servicePeerReceive,
			IServicePeerSend servicePeerSend,
			LoginInfo loginInfo,
			IScheduler scheduler,
			INoSql noSql,
			ILogger<ChatHost> logger)
		{
			this.servicePeerSend = servicePeerSend;
			this.loginInfo = loginInfo;
			this.scheduler = scheduler;
			this.noSql = noSql;
			this.logger = logger;

			servicePeerReceive.AddCommandHandler((short)ServiceCommand.SendMessage, OnSendMessageReq);
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			logger.LogInformation("1. StartAsync has been called.");

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			logger.LogInformation("4. StopAsync has been called.");

			return Task.CompletedTask;
		}

		private void OnSendMessageReq(int serialNumber, byte[] protoData)
		{
			scheduler.Enqueue(() =>
			{
				(bool ok, string name) = loginInfo.GetPlayerName(serialNumber);

				SendMessageReq req = ProtocolBuffConvert.Deserialize<SendMessageReq>(protoData.ToArray());

				SendMessageResult result = SendMessageResult.Failed;
				if (ok)
				{
					result = SendMessageResult.Ok;
				}

				ChatNoSql chatNoSql = new ChatNoSql()
				{
					Name = name,
					Text = req.Text,
				};

				noSql.Add(ChatNoSql.Key, chatNoSql);

				SendMessageResp sendMessageResp = new SendMessageResp()
				{
					Name = name,
					Text = req.Text,
					Result = result,
				};
				servicePeerSend.SendMessageToAllAsync((short)ServiceCommand.SendMessage, sendMessageResp).UndeadlockResult();
				logger.LogInformation($"SendMessageResult {result}, {name} : {req.Text}");
			});
		}
	}
}
