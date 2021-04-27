namespace PaymentProvider.ApiResources
{
    public enum PaymentTransactionStatus
    {
        Init,
        Pending,
        Approved,
        Declined,
        DeclinedDueToInvalidCreditCard
    }
}
