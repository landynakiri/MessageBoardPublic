using System.Reflection;

namespace Landy.DependencyInjection
{
	public class Host
	{
		private static HostBuilder _hostBuilder;

		public static IHostBuilder CreateDefaultBuilder()
		{
			_hostBuilder = new HostBuilder();
			return _hostBuilder;
		}

        public static void ExecuteInject<T>(T implementationInstance) where T : class
        {
            FieldInfo[] fields = implementationInstance.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo field in fields)
            {
                object[] attrs = field.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    InjectionAttribute injectionAttr = attr as InjectionAttribute;
                    if (injectionAttr != null)
                    {   
                        object ob = _hostBuilder.GetService(field.FieldType);
                        field.SetValue(implementationInstance, ob);
                    }
                }
            }
        }
    }
}
