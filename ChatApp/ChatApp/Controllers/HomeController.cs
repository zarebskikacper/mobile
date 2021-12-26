using ChatApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ChatApp.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public const string SessionKeyImie = "USER_SESSION_IMIE";
    public const string SessionKeyNazwisko = "USER_SESSION_NAZWISKO";

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult SignIn()
    {
        var login = HttpContext.Session.GetString(SessionKeyImie);
        var login2 = HttpContext.Session.GetString(SessionKeyNazwisko);

        if (login != null && login2 != null)
        {
            return RedirectToAction("ChatApp");
        }

        var user = new User();
        return View(user);
    }

    [HttpPost]
    public IActionResult SignIn(User user)
    {
        if (!ModelState.IsValid)
        {
            return View(user);
        }
        //var savedUser = HttpContext.Session.GetString(SessionKey);
        HttpContext.Session.SetString(SessionKeyImie, user.Imie);
        HttpContext.Session.SetString(SessionKeyNazwisko, user.Nazwisko);
        return RedirectToAction("ChatApp");
    }

    [HttpGet]
    public IActionResult LogOut()
    {
        HttpContext.Session.Remove(SessionKeyImie);
        HttpContext.Session.Remove(SessionKeyNazwisko);

        return RedirectToAction("SignIn");
    }

    public IActionResult ChatApp()
    {
        var login = HttpContext.Session.GetString(SessionKeyImie);
        var login2 = HttpContext.Session.GetString(SessionKeyNazwisko);

        if (login == null || login2 == null)
        {
            return RedirectToAction("SignIn");
        }

        var vm = new User()
        {
            Imie = login,
            Nazwisko = login2
        };
        
        return View(vm);
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
