using System;
using Google.Protobuf;

namespace Test.Common
{
	public class ProtocolBuffConvert
	{
		public static byte[] Serialize<T>(T obj) where T : IMessage
		{
			byte[] data = obj.ToByteArray();
			return data;
		}

		public static T Deserialize<T>(byte[] data) where T : class, IMessage, new()
		{
			T obj = new T();
			try
			{
				IMessage message = obj.Descriptor.Parser.ParseFrom(data);

				return message as T;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}

			return obj;
		}
	}
}
