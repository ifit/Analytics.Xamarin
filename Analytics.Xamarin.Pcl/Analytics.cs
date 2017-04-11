
namespace Segment
{
	public class Analytics
	{
		// REMINDER: don't forget to set Properties.AssemblyInfo.AssemblyVersion as well
		public static string VERSION = "1.0.0";

		public static IClient Client { get; private set; }

		/// <summary>
		/// Initialized the default Segment.io client with your API writeKey.
		/// </summary>
		/// <param name="writeKey"></param>
		public static void Initialize(string writeKey)
		{
			if (Client == null)
			{
				Client = new Client(writeKey);
			}
		}

		/// <summary>
		/// Initialized the default Segment.io client with your API writeKey.
		/// </summary>
		/// <param name="writeKey"></param>
		public static void Initialize(string writeKey, Config config)
		{
			if (Client == null)
			{
				Client = new Client(writeKey, config);
			}
		}
	}
}
