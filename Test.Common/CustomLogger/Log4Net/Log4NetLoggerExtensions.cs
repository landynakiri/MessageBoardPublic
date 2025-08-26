using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace Test.Common.CustomLogger.Log4Net
{
	public static class Log4NetLoggerExtensions
	{
		public static ILoggingBuilder AddLog4NetLogger(
			this ILoggingBuilder builder)
		{
			builder.AddConfiguration();

			builder.Services.TryAddEnumerable(
				ServiceDescriptor.Singleton<ILoggerProvider, Log4NetLoggerProvider>());

			LoggerProviderOptions.RegisterProviderOptions
				<Log4NetLoggerConfiguration, Log4NetLoggerProvider>(builder.Services);

			return builder;
		}

		public static ILoggingBuilder AddLog4NetLogger(
			this ILoggingBuilder builder,
			Action<Log4NetLoggerConfiguration> configure)
		{
			builder.AddLog4NetLogger();
			builder.Services.Configure(configure);

			return builder;
		}
	}
}
