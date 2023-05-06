using Microsoft.AspNetCore.Mvc;

namespace NotificationBot.Controllers;

[Route("/")]
public class HomeController: Controller {

    public ActionResult Index()
    {
        return View();
    }
    
    [HttpGet]
    [Route("discord")]
    public string RedirectToDiscord() {
        return "https://discord.com/oauth2/authorize?client_id=1098759533750394971&permissions=3072&scope=bot";
    }
}