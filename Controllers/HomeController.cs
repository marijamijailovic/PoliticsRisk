using Microsoft.AspNetCore.Mvc;

namespace PoliticsRisk.Controllers{
    public class HomeController : Controller{
        public ViewResult Index(){
            return View();
        }

        public ViewResult Menu(){
            return View();
        }
    }
}