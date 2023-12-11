using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace MvcMovie.Controllers
{
    public class HelloWorldController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Welcome(string name, int numTimes = 1)
        {
            //HtmlEncoder.Default.Encode previne BO, coisas má intencionada.
            //O $ indica que vai ter coisa inserida, um parâmetro.
            // return HtmlEncoder.Default.Encode($"Oi {name}, ID: {ID}");

            ViewData["Message"] = "Oi " + name;
            ViewData["NumTimes"] = numTimes;
            return View();

        }
    }
}
