using CustomResponse;
using CustomResponse.Models;
using NuGet.Protocol;

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
            HttpResponseMessage response = await _httpClient.GetAsync(options.FullUrl);
            return GetRequestResult(response);
        }

        public async Task<Result> Get(FetchRequestOptions options)
        {
            options.BaseUrl = string.IsNullOrEmpty(options.BaseUrl) ? _options.BaseUrl : options.BaseUrl;
            AddHeaders(options.Headers);
            HttpResponseMessage response = await _httpClient.GetAsync(options.FullUrl);
            return GetRequestResult(response);
        }

        public async Task<Result> Post(FetchRequestOptions options)
        {
            options.BaseUrl = string.IsNullOrEmpty(options.BaseUrl) ? _options.BaseUrl : options.BaseUrl;
            AddHeaders(options.Headers);
            HttpResponseMessage response = await _httpClient.PostAsync(options.FullUrl, options.Content);
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
            HttpResponseMessage response = await _httpClient.PostAsync(options.FullUrl, options.Content);
            return GetRequestResult(response);
        }

        public async Task<T?> GetData<T>(object? res)
        {
            if (res is null)
                return default;

            HttpResponseMessage response = res as HttpResponseMessage;
            string responseBody = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(responseBody))
                return default;

            T data = responseBody.FromJson<T>();
            return data;
        }

        private void AddHeaders(List<FetchHttpHeader>? headers)
        {
            if (headers != null)
                foreach (var header in headers)
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
        }

        private Result GetRequestResult(HttpResponseMessage response)
        {
            int statusCode = (int)response.StatusCode;

            if (response.IsSuccessStatusCode)
                return CustomResults.HttpRequestOk(response, statusCode);
            return CustomErrors.HttpRequestFailed(response, statusCode);
        }

    }
}