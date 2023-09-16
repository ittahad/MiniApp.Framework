using MiniApp.Core;
using System.Text.Json;

namespace MiniApp.Http
{
    public class MinimalHttpClient : IMinimalHttpClient
    {
        private readonly HttpClient _httpClient;

        public MinimalHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TResponse> MakeHttpRequest<TResponse>(HttpRequestMessage httpRequestMessage)
        {
            var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var data = await httpResponseMessage.Content.ReadAsStringAsync();
                if (typeof(TResponse) == typeof(string) && data != null)
                {
                    var sr = JsonSerializer.Serialize(data);
                    return JsonSerializer.Deserialize<TResponse>(sr);
                }
                TResponse? parsedObject = TryParsingTheResponse<TResponse>(data);

                return parsedObject;
            }

            return default;
        }

        private static TResponse? TryParsingTheResponse<TResponse>(string data)
        {
            try
            {
                return JsonSerializer.Deserialize<TResponse>(data);
            }
            catch (Exception ex)
            {
                return default;
            }
        }
    }
}
