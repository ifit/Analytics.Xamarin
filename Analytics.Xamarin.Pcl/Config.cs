using System;

namespace Segment
{
	/// <summary>
	/// Config required to initialize the client
	/// </summary>
	public class Config
	{
		/// <summary>
		/// The REST API endpoint
		/// </summary>
		public string Host { get; private set; }

		public TimeSpan Timeout { get; private set; }

		public Config() : this(Defaults.Host, Defaults.Timeout) { }

		public Config(TimeSpan timeout) : this(Defaults.Host, timeout) { }

		public Config(string host) : this(host, Defaults.Timeout) { }

		public Config(string host, TimeSpan timeout)
		{
			this.Host = host;
			this.Timeout = timeout;
		}

		/// <summary>
		/// Sets the maximum amount of timeout on the HTTP request flushes to the server.
		/// </summary>
		/// <param name="timeout"></param>
		/// <returns></returns>
		public Config SetTimeout(TimeSpan timeout)
		{
			this.Timeout = timeout;
			return this;
		}
	}
}
