using System.Threading.Tasks;

using PaymentProvider.ApiResources;

namespace PaymentProvider.HttpClient
{
    public interface IPaymentProviderHttpClient
    {
        Task<CreatePaymentResponse> CreatePayment(CreatePaymentRequest request);

        Task<PaymentStatusResponse> ApprovePayment(ApprovePaymentRequest request);

        Task<PaymentStatusResponse> GetStatus(string transactionId);
    }
}
