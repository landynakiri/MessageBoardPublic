using Microsoft.Extensions.DependencyInjection;

namespace Landy.DependencyInjection
{
	public class Demo
	{
		public void Start()
		{
			Host.CreateDefaultBuilder()
				.ConfigureServices(services =>
				{
					services.AddSingleton<Sample1>();
					services.AddSingleton<Sample2>();
				}).Build<DemoStartup>();
		}
	}
}
