using System.Dynamic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using PaymentProvider.WebApp.Models;
using PaymentProvider.WebApp.Services;

namespace PaymentProvider.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPaymentService _paymentService;

        public HomeController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]Payment payment)
        {
            var result = await _paymentService.CreatePayment(payment);
            dynamic viewModel = new ExpandoObject();
            viewModel.Payment = result;
            viewModel.RedirectUrl = $"{Request.Scheme}://{Request.Host}{Url.Action("Confirm")}";
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Confirm(string md, string paRes)
        {
            var result = await _paymentService.ApprovePayment(md, paRes);

            return result == null ? (IActionResult)BadRequest() : View(result);
        }
    }
}
