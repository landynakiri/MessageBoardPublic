namespace Test.Server.SQLService.Table
{
	public class Account : IPrimaryKeyContext<string>
	{
		public string ID { get; set; }
		public string Password { get; set; }
	}
}
