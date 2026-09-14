using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace SUP.RPCClient
{
    //Courtesy mb300sd Bitcoin.NET
	public partial class CoinRPC
	{
		protected Uri uri;

		protected NetworkCredential credentials;

		public CoinRPC(Uri _Uri, NetworkCredential _Credentials)
		{
			uri = _Uri;
			credentials = _Credentials;
		}

		protected string HttpCall(string jsonRequest)
		{
            try
            {
               using var client = new HttpClient();
               using var request = new HttpRequestMessage(HttpMethod.Post, uri);
               request.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-rpc");

               string auth = credentials.UserName + ":" + credentials.Password;
               auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(auth), Base64FormattingOptions.None);
               request.Headers.Authorization = new AuthenticationHeaderValue("Basic", auth);

               using HttpResponseMessage response = client.Send(request);
               string responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

               if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.InternalServerError)
               {
                   return responseContent;
               }

               throw new HttpRequestException($"RPC request failed with status code {(int)response.StatusCode} ({response.StatusCode}).");
            }
            catch (HttpRequestException hex)
            {
             return "{400:\"" + hex.Message +"\"}";
            }
		}

		private T RpcCall<T>(RPCRequest rpcRequest)
		{
			string jsonRequest = JsonConvert.SerializeObject(rpcRequest);
            string result = HttpCall(jsonRequest);

			RPCResponse<T> rpcResponse = JsonConvert.DeserializeObject<RPCResponse<T>>(result);

			if (rpcResponse.error != null)
			{
				throw new CoinRPCException(rpcResponse.error);
			}
			return rpcResponse.result;
		}
	}
}
