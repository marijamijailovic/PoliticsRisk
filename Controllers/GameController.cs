using Microsoft.AspNetCore.Mvc;

namespace PoliticsRisk.Controllers{
    public class GameController : Controller{

        public ViewResult GameMenu(){
            return View();
        }

        public ViewResult GameChat(){
            return View();
        }

        public ViewResult GameMaps(){
            return View();
        }
    }
}