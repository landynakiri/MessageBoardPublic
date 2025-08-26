namespace Test.Server.Common.Application
{
	public interface INoSql
	{
		void Add<T>(string key, T data) where T : class;
	}
}
