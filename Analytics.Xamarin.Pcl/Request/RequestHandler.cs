using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Connectivity;
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
		private Uri _host;

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
			this._host = new Uri(host);
			this._client = new HttpClient();
			this._client.Timeout = timeout;

			// do not use the expect 100-continue behavior
			this._client.DefaultRequestHeaders.ExpectContinue = false;
		}

		public async Task<bool> Process (BaseAction action, LogDelegate logger)
		{
            try {
                if (CrossConnectivity.Current.IsConnected) {
                    await Send(action);
                } else {
                    return false;
                }
            } catch (Exception ex) {
				if (null != logger) {
					logger($"Analytics call failed: {ex.Message}");
				}
                return false;
			}

            return true;
		}

		private async Task Send (BaseAction action)
		{
			var batch = new Batch (WriteKey, new List<BaseAction> () { action });

			// set the current request time
			batch.SentAt = DateTime.Now.ToString ("o");
			var json = JsonConvert.SerializeObject (batch);

			var uri = new Uri(_host, "/v1/import");

			var request = new HttpRequestMessage (HttpMethod.Post, uri);

			// basic auth: https://segment.io/docs/tracking-api/reference/#authentication
			request.Headers.Add ("Authorization", BasicAuthHeader (batch.WriteKey, ""));
			request.Content = new StringContent (json, Encoding.UTF8, "application/json");

			var start = DateTime.Now;

			var response = await _client.SendAsync (request);

			if (!response.IsSuccessStatusCode) {
				throw new WebException ($"Segment API request returned an unexpected status code: {response.StatusCode} {response.Content}");
			}
		}
		
		private string BasicAuthHeader(string user, string pass)
		{
			return "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{user}:{pass}"));
		}

		public void Dispose ()
		{
			_client.Dispose ();
		}

		#endregion //Methods
	}
}
