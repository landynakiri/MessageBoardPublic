using System;
using System.Collections.Concurrent;
using System.IO;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Test.Common.CustomLogger.Log4Net
{
	[ProviderAlias("Log4Net")]
	public sealed class Log4NetLoggerProvider : ILoggerProvider
	{
		private readonly IDisposable _onChangeToken;
		private Log4NetLoggerConfiguration _currentConfig;
		private readonly ConcurrentDictionary<string, Log4NetLogger> _loggers = new ConcurrentDictionary<string, Log4NetLogger>(StringComparer.OrdinalIgnoreCase);

		public Log4NetLoggerProvider(
			IOptionsMonitor<Log4NetLoggerConfiguration> config)
		{
			_currentConfig = config.CurrentValue;
			_onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);

			DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
			FileInfo fileInfo = new FileInfo(Path.Combine(dir.Parent.Parent.FullName, "log4net.config"));
			log4net.Config.XmlConfigurator.Configure(fileInfo);
		}

		public ILogger CreateLogger(string categoryName) =>
			_loggers.GetOrAdd(categoryName, name => new Log4NetLogger(name, LogManager.GetLogger(categoryName), GetCurrentConfig));

		private Log4NetLoggerConfiguration GetCurrentConfig() => _currentConfig;

		public void Dispose()
		{
			_loggers.Clear();
			_onChangeToken?.Dispose();
		}
	}
}
