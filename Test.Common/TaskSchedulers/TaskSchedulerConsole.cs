using System;
using System.Threading.Tasks;

namespace Test.Common.TaskSchedulers
{
	public class TaskSchedulerConsole : IScheduler
	{
		private readonly TaskFactory fac = new TaskFactory(new QueuedTaskScheduler());

		public Task Enqueue(Action action)
		{
			return fac.StartNew(action);
		}

		public Task Enqueue(Action<object> action, object state)
		{
			return fac.StartNew(action, state);
		}
	}
}
