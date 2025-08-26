using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Test.Client.Common.Application;

namespace Test.Client.UI
{
	public class UIController : IUIController
	{
		private Dictionary<string, IUIEvent> views = new Dictionary<string, IUIEvent>();

		private readonly ILogger<UIController> logger;

		public UIController(MainForm mainForm, ILogger<UIController> logger)
		{
			views.Add(UINames.MainForm.ToString(), mainForm);
			this.logger = logger;
		}

		public object GetView<T>(UINames uiNames) where T : class
		{
			if (views.TryGetValue(uiNames.ToString(), out var view))
			{
				return view;
			}
			else
			{
				logger.LogError($"Cannot found view [{uiNames.ToString()}]");
				return null;
			}
		}

		public void RegisterEvent<T>(UINames uiNames, T obj) where T : IUI
		{
			if (views.TryGetValue(uiNames.ToString(), out IUIEvent viewEvent))
			{
				viewEvent.RegisterEvent(obj);

				return;
			}

			throw new Exception($"Cannot found view {uiNames.ToString()}");
		}

		public void UnregisterEvent<T>(UINames uiNames, T obj) where T : IUI
		{
			if (views.TryGetValue(uiNames.ToString(), out IUIEvent viewEvent))
			{
				viewEvent.UnregisterEvent(obj);

				return;
			}

			throw new Exception($"Cannot found view {uiNames.ToString()}");
		}
	}
}
