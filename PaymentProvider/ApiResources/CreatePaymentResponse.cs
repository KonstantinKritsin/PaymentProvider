namespace PaymentProvider.ApiResources
{
    public class CreatePaymentResponse
    {
        public string TransactionId { get; set; }

        public PaymentTransactionStatus TransactionStatus { get; set; }

        public string PaReq { get; set; }

        public string Url { get; set; }

        public string Method { get; set; }
    }
}
