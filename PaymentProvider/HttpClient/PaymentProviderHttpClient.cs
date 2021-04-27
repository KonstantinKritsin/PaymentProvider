using System;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using PaymentProvider.ApiResources;

namespace PaymentProvider.HttpClient
{
    internal class PaymentProviderHttpClient : IPaymentProviderHttpClient
    {
        private readonly System.Net.Http.HttpClient _client;
        private readonly ILogger<PaymentProviderHttpClient> _logger;

        public PaymentProviderHttpClient(ILogger<PaymentProviderHttpClient> logger, System.Net.Http.HttpClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<CreatePaymentResponse> CreatePayment(CreatePaymentRequest request)
        {
            var result = await Post<CreatePaymentRequest, CreatePaymentResponse>("api/payment/create", request);
            // because returned from service url is not working??
            result.Url = "http://dumdumpay.site/secure/";
            return result;
        }

        public Task<PaymentStatusResponse> ApprovePayment(ApprovePaymentRequest request) =>
            Post<ApprovePaymentRequest, PaymentStatusResponse>("api/payment/confirm", request);

        public Task<PaymentStatusResponse> GetStatus(string transactionId) => Get<PaymentStatusResponse>($"payment/{transactionId}/status");

        private Task<TResponse> Post<TRequest, TResponse>(string url, TRequest request)
            where TRequest : class =>
            ProcessRequest<TRequest, TResponse>(HttpMethod.Post, url, request);

        private Task<TResponse> Get<TResponse>(string url) => ProcessRequest<string, TResponse>(HttpMethod.Get, url);

        private async Task<TResponse> ProcessRequest<TRequest, TResponse>(HttpMethod method, string url, TRequest request = null)
            where TRequest : class
        {
            var requestContent = request == null
                ? null
                : JsonConvert.SerializeObject(request);

            var responseString = await SendRequest(method, url, requestContent);

            ResponseWrapper<TResponse> response;

            try
            {
                response = JsonConvert.DeserializeObject<ResponseWrapper<TResponse>>(responseString);
            }
            catch (Exception e)
            {
                var message = $"Unable to deserialize response '{responseString}'";
                _logger.LogError(e, message);
                throw new PaymentException(message, e);
            }

            if (response.Errors?.Length > 0)
            {
                var cumulativeError = string.Join(Environment.NewLine, response.Errors?.Select(e => e.ToString()));
                var errorMessage = $"Payment request returns errors: {cumulativeError}";
                _logger.LogError(errorMessage);
                throw new PaymentException(errorMessage);
            }

            return response.Result;
        }

        private async Task<string> SendRequest(HttpMethod method, string url, string content)
        {
            try
            {
                using (var httpRequestMessage = new HttpRequestMessage(method, url))
                {
                    if (!string.IsNullOrEmpty(content))
                    {
                        httpRequestMessage.Content = new StringContent(content, Encoding.Default, MediaTypeNames.Application.Json);
                    }

                    using (var responseMessage = await _client.SendAsync(httpRequestMessage))
                    {
                        responseMessage.EnsureSuccessStatusCode();
                        return await responseMessage.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (HttpRequestException httpRequestException)
            {
                _logger.LogError(httpRequestException, "Unable to make a payment request.");
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error.");
                throw;
            }
        }

        private struct ResponseWrapper<T>
        {
            public T Result { get; set; }

            public ApiError[] Errors { get; set; }
        }
    }
}
