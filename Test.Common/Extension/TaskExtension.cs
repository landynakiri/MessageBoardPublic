namespace System.Threading.Tasks
{
	public static class TaskExtension
	{
		public static T UndeadlockResult<T>(this Task<T> task)
		{
			return task.ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public static void UndeadlockResult(this Task task)
		{
			task.ConfigureAwait(false);
		}
	}
}
