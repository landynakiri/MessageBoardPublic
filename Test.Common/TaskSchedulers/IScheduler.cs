using System;
using System.Threading.Tasks;

namespace Test.Common.TaskSchedulers
{
	public interface IScheduler
	{
		Task Enqueue(Action action);
		Task Enqueue(Action<object> action, object state);
	}
}
