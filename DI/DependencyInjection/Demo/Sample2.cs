using System;

namespace Landy.DependencyInjection
{
	public class Sample2 : IDisposable
	{
		public Sample2(Sample1 sample1)
		{
			Console.WriteLine("Create Sample2");
		}

		public void Dispose()
		{
			Console.WriteLine("Dispose Sample2");
		}
	}
}
