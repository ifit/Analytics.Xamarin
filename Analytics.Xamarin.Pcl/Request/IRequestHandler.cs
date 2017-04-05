using System;
using System.Threading.Tasks;
using Segment.Model;

namespace Segment.Request
{
	internal interface IRequestHandler : IDisposable
	{
		/// <summary>
		/// Send an action batch to the Segment tracking API.
		/// </summary>
		/// <param name="batch">Batch.</param>
		Task SendBatch(Batch batch);
	}
}
