using System;

using Microsoft.Extensions.DependencyInjection;

using PaymentProvider.Handlers;
using PaymentProvider.HttpClient;
using PaymentProvider.Options;

namespace PaymentProvider.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPaymentProvider(this IServiceCollection services, PaymentProviderOptions options)
        {
            var clientBuilder = services
                .AddSingleton(options)
                .AddTransient<IPaymentProviderHttpClient, PaymentProviderHttpClient>()
                .AddHttpClient<IPaymentProviderHttpClient, PaymentProviderHttpClient>()
                .ConfigureHttpClient(client => client.BaseAddress = new Uri(options.Host));

            if (options.Authentication != null)
            {
                services.AddTransient<PaymentProviderAuthenticationHandler>();
                clientBuilder.AddHttpMessageHandler<PaymentProviderAuthenticationHandler>();
            }

            return services;
        }
    }
}
