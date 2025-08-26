using Test.Client.Common.Application;

namespace Test.Client.Chat.Application
{
	internal interface IChatSend : IUI
	{
		void OnClickSendMessage();
	}
}
