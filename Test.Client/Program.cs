using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Test.Client.Chat.Application;
using Test.Client.Chat.Domain;
using Test.Client.Client;
using Test.Client.Common.Application;
using Test.Client.Login.Application;
using Test.Client.Login.Domain;
using Test.Client.UI;
using Test.Client.View;
using Test.Common.CustomLogger.Log4Net;
using Test.Common.TaskSchedulers;
using Test.Common.Tcp.ConnectObserver;
using Test.Common.Tcp.ServicePeer;

namespace Test.Client
{
	internal static class Program
	{
		/// <summary>
		/// 應用程式的主要進入點。
		/// </summary>
		//[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			IHost host = Host.CreateDefaultBuilder()
				.ConfigureLogging((hostContext, logging) =>
				{
					logging.ClearProviders();
					logging.AddLog4NetLogger();
				})
				.ConfigureServices(services =>
				{
					IServicePeerReceive servicePeerReceive = new ServicePeerReceive();
					services.AddSingleton<IServicePeerReceive>(servicePeerReceive);
					ConnectStatusMonitor connectMonitor = new ConnectStatusMonitor();
					services.AddSingleton<IObservable<ConnectInfo>>(connectMonitor);
					var logger = services.BuildServiceProvider().GetService<ILogger<TcpConnector>>();
					services.AddSingleton<TcpConnector>(new TcpConnector(GlobalSetting.IP, GlobalSetting.Port, servicePeerReceive, connectMonitor, logger));

					AddViewServices(services);
					services.AddSingleton<IUIController, UIController>();

					services.AddSingleton<IScheduler, TaskSchedulerConsole>();

					//DomainData
					services.AddSingleton<LoginInfo>();
					services.AddSingleton<Player>();
					services.AddSingleton<MainInfo>();

					//Application
					services.AddSingleton<LoginHost>();
					services.AddSingleton<ChatHost>();

					services.AddSingleton<StartUpClient>();
				}).Build();

			var serviceProvider = host.Services;
			StartUpClient startUpClient = serviceProvider.GetService<StartUpClient>();
			startUpClient.Start();
			Application.Run(serviceProvider.GetService<MainForm>());
			startUpClient.Stop();
		}

		private static void AddViewServices(IServiceCollection services)
		{
			//Add all forms
			var forms = typeof(Program).Assembly
			.GetTypes()
			.Where(t => t.BaseType == typeof(BaseForm))
			.ToList();

			forms.ForEach(form =>
			{
				services.AddSingleton(form);
			});
		}
	}
}
