using System;
using System.Collections.Generic;

namespace Test.Common.Observer
{
	public class BaseObserver<T> : IObservable<T> where T : class
	{
		protected List<IObserver<T>> Observers;

		public BaseObserver()
		{
			Observers = new List<IObserver<T>>();
		}

		private class Unsubscriber<K> : IDisposable where K : class
		{
			private List<IObserver<K>> _observers;
			private IObserver<K> _observer;

			public Unsubscriber(List<IObserver<K>> observers, IObserver<K> observer)
			{
				_observers = observers;
				_observer = observer;
			}

			public void Dispose()
			{
				if (!(_observer == null))
				{
					_observers.Remove(_observer);
				}
			}
		}

		IDisposable IObservable<T>.Subscribe(IObserver<T> observer)
		{
			if (!Observers.Contains(observer))
			{
				Observers.Add(observer);
			}

			return new Unsubscriber<T>(Observers, observer);
		}
	}
}
