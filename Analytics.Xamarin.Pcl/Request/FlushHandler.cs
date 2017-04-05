using System.Collections.Generic;
using System.Threading.Tasks;
using Segment.Model;

namespace Segment.Request
{
	internal class FlushHandler : IFlushHandler
	{
		/// <summary>
		/// Performs the actual HTTP request to our server
		/// </summary>
		private IRequestHandler RequestHandler { get; set; }

		private string WriteKey { get; set; }

		internal FlushHandler(string writeKey, IRequestHandler requestHandler)
		{
			WriteKey = writeKey;
			RequestHandler = requestHandler;
		}

		public async Task Process(BaseAction action)
		{
			await RequestHandler.SendBatch(new Batch(WriteKey, new List<BaseAction> () { action }));
		}
	}
}
