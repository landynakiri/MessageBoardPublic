using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Test.Client.Common.Application;
using Test.Client.UI;

namespace Test.Client.View
{
	public class BaseForm : Form, IUIEvent
	{
		private HashSet<IUI> views = new HashSet<IUI>();

		public void RegisterEvent<T>(T obj) where T : IUI
		{
			views.Add(obj);
		}

		public void UnregisterEvent<T>(T obj) where T : IUI
		{
			views.Remove(obj);
		}

		public void Invoke<T>(Action<T> action) where T : class, IUI 
		{
			foreach (var view in views)
			{
				if(view is T type)
				{
					action.Invoke(type);
				}
			}
		}
	}
}
