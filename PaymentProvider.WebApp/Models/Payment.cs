namespace PaymentProvider.WebApp.Models
{
    public class Payment
    {
        public decimal Amount { get; set; }

        public string CardNumber { get; set; }

        public string CardHolder { get; set; }

        public int ExpireMonth { get; set; }

        public int ExpireYear { get; set; }

        public string Cvv { get; set; }
    }
}
