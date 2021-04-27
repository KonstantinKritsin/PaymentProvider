namespace PaymentProvider.ApiResources
{
    public class PaymentStatusResponse
    {
        public string TransactionId { get; set; }

        public PaymentTransactionStatus Status { get; set; }

        public string OrderId { get; set; }

        public int Amount { get; set; }

        public string Currency { get; set; }

        public string LastFourDigits { get; set; }
    }
}
