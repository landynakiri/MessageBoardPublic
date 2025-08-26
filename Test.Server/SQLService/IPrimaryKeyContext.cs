namespace Test.Server.SQLService
{
	public interface IPrimaryKeyContext<T>
	{
		T ID { get; set; }
	}
}
