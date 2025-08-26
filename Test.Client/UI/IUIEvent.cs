using Test.Client.Common.Application;

namespace Test.Client.UI
{
	public interface IUIEvent
	{
		void RegisterEvent<T>(T obj) where T : IUI;
		void UnregisterEvent<T>(T obj) where T : IUI;
	}
}
