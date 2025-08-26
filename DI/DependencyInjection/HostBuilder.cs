using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Landy.DependencyInjection
{
	public class HostBuilder : IHostBuilder
	{
		private IServiceProvider _appServices;

		private List<Action<IServiceCollection>> _configureServicesActions = new List<Action<IServiceCollection>>();

		//public IHostServiceBuilder CreateDefaultBuilder()
		//{

		//	return this;
		//}

		public IHostBuilder ConfigureServices(Action<IServiceCollection> configureDelegate)
		{
			_configureServicesActions.Add(configureDelegate);
			return this;
		}

		public IHostBuilder Build<T>() where T : class
		{
			CreateServiceProvider<T>();

			T startUp = _appServices.GetService<T>();

			var method = startUp.GetType().GetMethod("Start");
			method?.Invoke(startUp, null);

			return this;
		}
		private void CreateServiceProvider<T>() where T : class
		{
			ServiceCollection serviceCollection = new ServiceCollection();

			foreach (Action<IServiceCollection> configureServicesAction in _configureServicesActions)
			{
				configureServicesAction(serviceCollection);
			}

			serviceCollection.AddSingleton<T>();
			_appServices = serviceCollection.BuildServiceProvider();
		}

		public object GetService(Type type)
		{
			return _appServices.GetService(type);
		}

		public IHostBuilder Stop()
		{
			(_appServices as ServiceProvider).Dispose();

			return this;
		}
	}
}
