using Microsoft.AspNetCore.Mvc;

namespace SkincareBookingSystem.API.Controllers;

public class AuthContoller : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}