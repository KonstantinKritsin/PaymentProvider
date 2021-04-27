using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using PaymentProvider.Options;

namespace PaymentProvider.Handlers
{
    internal class PaymentProviderAuthenticationHandler : DelegatingHandler
    {
        private readonly PaymentAuthentication authentication;

        public PaymentProviderAuthenticationHandler(PaymentProviderOptions options)
        {
            authentication = options.Authentication;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Mechant-Id", authentication.MerchantId);
            request.Headers.Add("Secret-Key", authentication.SecretKey);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
