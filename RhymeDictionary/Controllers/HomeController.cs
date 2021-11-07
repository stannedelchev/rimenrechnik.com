using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RhymeDictionary.Helpers;
using RhymeDictionary.Model;
using RhymeDictionary.Model.Services;
using RhymeDictionary.ViewModels;

namespace RhymeDictionary.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRhymesService rhymesService;
        private readonly IStatisticsService statisticsService;

        public HomeController(IRhymesService rhymesService, IStatisticsService statisticsService)
        {
            this.rhymesService = rhymesService;
            this.statisticsService = statisticsService;
        }

        public IActionResult Index(string word)
        {
            this.ViewData[Constants.ActivePageViewDataId] = Constants.RhymeDictionaryNavbarId;

            if (string.IsNullOrEmpty(word?.Trim()))
            {
                return this.View();
            }

            return this.Redirect($"/{WebUtility.UrlEncode("рими")}/{Uri.EscapeUriString(word.Trim())}");
        }

        [Route("/рими/{word}")]
        public async Task<IActionResult> Rhymes(string word)
        {
            this.ViewData[Constants.ActivePageViewDataId] = Constants.RhymeDictionaryNavbarId;

            if (string.IsNullOrEmpty(word?.Trim()))
            {
                return this.View("Index");
            }

#pragma warning disable SA1312 // Variable names must begin with lower-case letter
            var _ = this.statisticsService.AddSearchVisitAsync(word, this.HttpContext.Connection.RemoteIpAddress.ToString());
#pragma warning restore SA1312 // Variable names must begin with lower-case letter
            var rhymes = await this.rhymesService.GetRhymesAsync(word.Trim());
            var result = new RhymesViewModel(word.Trim(), rhymes);

            return this.View("Index", result);
        }

        public IActionResult Error()
        {
            return this.View();
        }
    }
}