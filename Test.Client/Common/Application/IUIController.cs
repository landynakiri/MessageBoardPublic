using System.Threading.Tasks;

namespace Test.Client.Common.Application
{
	public interface IUIController
	{
		object GetView<T>(UINames uiNames) where T : class;

		void RegisterEvent<T>(UINames uiNames, T obj) where T : IUI;
		void UnregisterEvent<T>(UINames uiNames, T obj) where T : IUI;
	}
}
