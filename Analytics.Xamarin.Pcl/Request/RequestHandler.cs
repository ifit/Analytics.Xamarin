using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Segment.Model;

namespace Segment.Request
{
	/// <summary>
	/// Performs the actual HTTP request to our server
	/// </summary>
	internal class RequestHandler : IRequestHandler
	{
		#region Properties

		/// <summary>
		/// Segment API endpoint.
		/// </summary>
		private string _host;

		/// <summary>
		/// Http client
		/// </summary>
		private HttpClient _client;

		private string WriteKey { get; set; }

		#endregion //Properties

		#region Methods

		internal RequestHandler(string writeKey, string host, TimeSpan timeout)
		{
			WriteKey = writeKey;
			this._host = host;
			this._client = new HttpClient();
			this._client.Timeout = timeout;

			// do not use the expect 100-continue behavior
			this._client.DefaultRequestHeaders.ExpectContinue = false;
		}

		public async Task Process (BaseAction action, LogDelegate logger)
		{
			try {
				await Send (action);
			} catch (Exception ex) {
				if (null != logger) {
					logger(string.Format ("Segment triggered a circuitbreaker exception: {0}", ex));
				}
			}
		}

		private async Task Send (BaseAction action)
		{
			var batch = new Batch (WriteKey, new List<BaseAction> () { action });

			// set the current request time
			batch.SentAt = DateTime.Now.ToString ("o");
			string json = JsonConvert.SerializeObject (batch);

			Uri uri = new Uri (_host + "/v1/import");

			HttpRequestMessage request = new HttpRequestMessage (HttpMethod.Post, uri);

			// basic auth: https://segment.io/docs/tracking-api/reference/#authentication
			request.Headers.Add ("Authorization", BasicAuthHeader (batch.WriteKey, ""));
			request.Content = new StringContent (json, Encoding.UTF8, "application/json");

			var start = DateTime.Now;

			var response = await _client.SendAsync (request);

			if (!response.IsSuccessStatusCode) {
				string reason = string.Format ("Status Code {0} ", response.StatusCode);
				reason += response.Content.ToString ();
				throw new WebException (string.Format ("Segment API request returned an unexpected status code: {0}", reason));
			}
		}
		
		private string BasicAuthHeader(string user, string pass)
		{
			string val = user + ":" + pass;
			return "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(val));
		}

		public void Dispose ()
		{
			_client.Dispose ();
		}

		public Task Process (BaseAction action)
		{
			throw new NotImplementedException ();
		}

		#endregion //Methods
	}
}
