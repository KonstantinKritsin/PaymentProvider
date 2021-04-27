using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

using PaymentProvider.ApiResources;
using PaymentProvider.HttpClient;
using PaymentProvider.WebApp.Models;

namespace PaymentProvider.WebApp.Services
{
    public class PaymentService : IPaymentService
    {
        private static readonly ConcurrentDictionary<string, Transaction> _transactionsStorage =
            new ConcurrentDictionary<string, Transaction>();

        private readonly IPaymentProviderHttpClient _paymentProviderHttpClient;

        public PaymentService(IPaymentProviderHttpClient paymentProviderHttpClient)
        {
            _paymentProviderHttpClient = paymentProviderHttpClient;
        }

        public async Task<CreatePayment> CreatePayment(Payment payment)
        {
            var request = new CreatePaymentRequest
                {
                    Amount = (int)(payment.Amount * 100),
                    CardExpiryDate = $"{payment.ExpireMonth:D2}{payment.ExpireYear:D2}",
                    CardHolder = payment.CardHolder,
                    CardNumber = payment.CardNumber,
                    Country = "CY",
                    Currency = "EUR",
                    OrderId = Guid.NewGuid().ToString("D"),
                    Cvv = payment.Cvv
                };

            var response = await _paymentProviderHttpClient.CreatePayment(request);

            _transactionsStorage.TryAdd(
                request.OrderId,
                new Transaction { TransactionId = response.TransactionId, Status = response.TransactionStatus });

            return new CreatePayment
                {
                    Method = response.Method,
                    OrderId = request.OrderId,
                    IssuerUrl = response.Url,
                    PaReq = response.PaReq
                };
        }

        public async Task<Transaction> ApprovePayment(string orderId, string paymentResponse)
        {
            if (!_transactionsStorage.TryGetValue(orderId, out var transaction) || transaction.Status != PaymentTransactionStatus.Init)
            {
                return null;
            }

            var response = await _paymentProviderHttpClient.ApprovePayment(
                new ApprovePaymentRequest { TransactionId = transaction.TransactionId, PaRes = paymentResponse });

            var newTransaction = new Transaction { TransactionId = transaction.TransactionId, Status = response.Status };
            _transactionsStorage.TryUpdate(
                orderId,
                newTransaction,
                transaction);

            return newTransaction;
        }
    }
}
