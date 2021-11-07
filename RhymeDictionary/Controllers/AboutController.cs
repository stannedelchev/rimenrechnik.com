using Microsoft.AspNetCore.Mvc;
using RhymeDictionary.Helpers;

namespace RhymeDictionary.Controllers
{
    [Route("за-нас")]
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            this.ViewData[Constants.ActivePageViewDataId] = Constants.AboutNavbarId;
            return this.View();
        }
    }
}
