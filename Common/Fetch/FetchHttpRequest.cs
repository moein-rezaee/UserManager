using System.Text.Json;
using CustomResponce;
using CustomResponce.Models;

namespace Fetch
{
    public class FetchHttpRequest
    {
        private readonly HttpClient _httpClient;
        private readonly FetchClientOptions _options;

        public static FetchHttpRequest GetInstance(IHttpClientFactory httpClientFactory, string BaseUrl)
        {
            FetchClientOptions fetchClientOptions = new()
            {
                BaseUrl = BaseUrl
            };
            return new(httpClientFactory, fetchClientOptions);
        }

        public static FetchHttpRequest GetInstance(
            IHttpClientFactory httpClientFactory,
            FetchClientOptions fetchClientOptions) => new(httpClientFactory, fetchClientOptions);


        public FetchHttpRequest(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public FetchHttpRequest(IHttpClientFactory httpClientFactory, FetchClientOptions options)
        {
            AddHeaders(options.Headers);
            _httpClient = httpClientFactory.CreateClient();
            _options = options;
        }

        public async Task<Result> Get(string url)
        {
            FetchRequestOptions options = new()
            {
                Url = url
            };
            options.BaseUrl = string.IsNullOrEmpty(options.BaseUrl) ? _options.BaseUrl : options.BaseUrl;
            AddHeaders(options.Headers);
            var response = await _httpClient.GetAsync(options.FullUrl);
            return GetRequestResult(response);
        }

        public async Task<Result> Get(FetchRequestOptions options)
        {
            options.BaseUrl = string.IsNullOrEmpty(options.BaseUrl) ? _options.BaseUrl : options.BaseUrl;
            AddHeaders(options.Headers);
            var response = await _httpClient.GetAsync(options.FullUrl);
            return GetRequestResult(response);
        }

        public async Task<Result> Post(FetchRequestOptions options)
        {
            options.BaseUrl = string.IsNullOrEmpty(options.BaseUrl) ? _options.BaseUrl : options.BaseUrl;
            AddHeaders(options.Headers);
            using var response = await _httpClient.PostAsync(options.FullUrl, options.Content);
            return GetRequestResult(response);
        }

        public async Task<Result> Post(string url, object? data = null)
        {
            FetchRequestOptions options = new()
            {
                Url = url,
                Data = data
            };
            options.BaseUrl = string.IsNullOrEmpty(options.BaseUrl) ? _options.BaseUrl : options.BaseUrl;
            AddHeaders(options.Headers);
            using var response = await _httpClient.PostAsync(options.FullUrl, options.Content);
            return GetRequestResult(response);
        }

        public async Task<T?> GetData<T>(HttpResponseMessage response)
        {
            using var contentStream = await response.Content.ReadAsStreamAsync();
            if (contentStream != null)
            {
                T? data = await JsonSerializer.DeserializeAsync<T>(contentStream);
                return data;
            }
            return default;
        }

        private void AddHeaders(List<FetchHttpHeader>? headers)
        {
            if (headers != null)
                foreach (var header in headers)
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
        }

        private Result GetRequestResult(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return CustomResults.HttpRequestOk(response);
            return CustomErrors.HttpRequestFailed(response);
        }

    }
}