using System;

namespace Landy.DependencyInjection
{
	public class SamepleMonobehaviour
	{
		[Injection]
		private Sample1 _sample1;

		public SamepleMonobehaviour()
		{
			Console.WriteLine(_sample1.ToString());
		}
	}
}
