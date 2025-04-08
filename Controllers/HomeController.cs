using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceChatbot.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("")]
        [HttpGet("Home")]
        [HttpGet("Home/Index")]
        [HttpGet("index.html")]
        public IActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pages", "index.html"), "text/html");
        }

        [HttpGet("pages/chat.html")]
        public IActionResult Chat()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pages", "chat.html"), "text/html");
        }

    }
}
