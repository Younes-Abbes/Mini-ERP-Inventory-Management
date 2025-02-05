using Microsoft.AspNetCore.Mvc;

namespace Mini_ERP.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet("Account/AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }

}
