using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RhymeDictionary.Helpers;
using RhymeDictionary.Model.Services;
using RhymeDictionary.ViewModels;

namespace RhymeDictionary.Controllers
{
    [Route("добави-рими")]
    public class AddRhymesController : Controller
    {
        private readonly IWordSuggestionsService suggestionsService;

        public AddRhymesController(IWordSuggestionsService suggestionsService, IStatisticsService statisticsService)
        {
            this.suggestionsService = suggestionsService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            this.ViewData[Constants.ActivePageViewDataId] = Constants.AddWordNavbarId;
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(AddWordsViewModel model)
        {
            this.ViewData[Constants.ActivePageViewDataId] = Constants.AddWordNavbarId;

            if (!this.ModelState.IsValid)
            {
                return this.View("Index", model);
            }

            var ipAddress = this.HttpContext.Connection.RemoteIpAddress.ToString();
            var suggestions = model?.Suggestions?.Trim();
            await this.suggestionsService.AddSuggestions(suggestions, ipAddress);

            return this.RedirectToAction(nameof(this.ThankYou));
        }

        [Route("благодарим-ви")]
        public IActionResult ThankYou()
        {
            this.ViewData[Constants.ActivePageViewDataId] = Constants.AddWordNavbarId;
            return this.View();
        }
    }
}
