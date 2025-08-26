using System;
using log4net;
using Microsoft.Extensions.Logging;

namespace Test.Common.CustomLogger.Log4Net
{
	public sealed class Log4NetLogger : ILogger
	{
		private readonly string _name;
		private readonly Func<Log4NetLoggerConfiguration> _getCurrentConfig;
		private readonly ILog log;

		public Log4NetLogger(string name,
			ILog log,
			Func<Log4NetLoggerConfiguration> getCurrentConfig)
		{
			(_name, _getCurrentConfig) = (name, getCurrentConfig);
			this.log = log;
		}

		public IDisposable BeginScope<TState>(TState state)
		{
			return default;
		}

		public bool IsEnabled(LogLevel logLevel) => true;

		public void Log<TState>(
			LogLevel logLevel,
			EventId eventId,
			TState state,
			Exception exception,
			Func<TState, Exception, string> formatter)
		{
			if (!IsEnabled(logLevel))
			{
				return;
			}

			Log4NetLoggerConfiguration config = _getCurrentConfig();
			if (config.EventId == 0 || config.EventId == eventId.Id)
			{
				string message = formatter(state, exception);
				switch (logLevel)
				{
					case LogLevel.Debug:
						log.Debug(message);
						return;
					case LogLevel.Information:
						log.Info(message);
						return;
					case LogLevel.Warning:
						log.Warn(message);
						return;
					case LogLevel.Error:
						log.Error(message);
						return;
					case LogLevel.Critical:
						log.Fatal(message);
						return;
					default:
						log.Warn(message);
						return;
				}
			}
		}
	}
}
