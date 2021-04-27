namespace PaymentProvider.WebApp.Models
{
    public class CreatePayment
    {
        public string OrderId { get; set; }

        public string Method { get; set; }

        public string IssuerUrl { get; set; }

        public string PaReq { get; set; }
    }
}
