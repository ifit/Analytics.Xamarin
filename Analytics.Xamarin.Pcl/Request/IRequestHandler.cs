using System;
using System.Threading.Tasks;
using Segment.Model;

namespace Segment.Request
{
	internal interface IRequestHandler : IDisposable
	{
		/// <summary>
		/// Validates an action and begins the process of flushing it to the server
		/// </summary>
		/// <param name="action">The action to send out</param>
		Task Process (BaseAction action, LogDelegate logger);
	}
}
