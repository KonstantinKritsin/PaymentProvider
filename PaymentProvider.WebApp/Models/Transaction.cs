using PaymentProvider.ApiResources;

namespace PaymentProvider.WebApp.Models
{
    public class Transaction
    {
        public string TransactionId { get; set; }

        public PaymentTransactionStatus Status { get; set; }
    }
}
