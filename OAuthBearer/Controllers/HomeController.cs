using System.Web.Mvc;

namespace KatanaOAuthBearer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "OAuthBearer using Katana";

            return View();
        }
    }
}
