using System;
using Microsoft.Extensions.DependencyInjection;

namespace Landy.DependencyInjection
{
	public interface IHostBuilder
	{
		IHostBuilder ConfigureServices(Action<IServiceCollection> configureDelegate);
		IHostBuilder Build<T>() where T : class;

		IHostBuilder Stop();
	}
}
