using Test.Client.Common.Application;

namespace Test.Client.Login.Application
{
	public interface ILogin : IUI
	{
		void OnClickLogin();
		void OnClickLogout();
	}
}
