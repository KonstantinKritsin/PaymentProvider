using System.Threading.Tasks;

using PaymentProvider.WebApp.Models;

namespace PaymentProvider.WebApp.Services
{
    public interface IPaymentService
    {
        Task<CreatePayment> CreatePayment(Payment payment);

        Task<Transaction> ApprovePayment(string orderId, string paymentResponse);
    }
}
