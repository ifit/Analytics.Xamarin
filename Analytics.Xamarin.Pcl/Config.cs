using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
		internal string Host { get; set; }

		internal int MaxQueueSize { get; set; }

		internal TimeSpan Timeout { get; set; }

		public Config()
		{
			this.Host = Defaults.Host;
			this.Timeout = Defaults.Timeout;
			this.MaxQueueSize = Defaults.MaxQueueCapacity;
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

		/// <summary>
		/// Sets the maximum amount of items that can be in the queue before no more are accepted.
		/// </summary>
		/// <param name="maxQueueSize"></param>
		/// <returns></returns>
		public Config SetMaxQueueSize(int maxQueueSize)
		{
			this.MaxQueueSize = maxQueueSize;
			return this;
		}
	}
}
