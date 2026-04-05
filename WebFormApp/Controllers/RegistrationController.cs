using Microsoft.AspNetCore.Mvc;
using WebFormApp.Models;

namespace WebFormApp.Controllers
{
    public class RegistrationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(UserModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Success = "Registration successful! Welcome, " + model.Name;
            }
            return View(model);
        }
    }
}