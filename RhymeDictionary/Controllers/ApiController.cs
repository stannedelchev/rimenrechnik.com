using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RhymeDictionary.Controllers.ViewModels;
using RhymeDictionary.Model.Services;

namespace RhymeDictionary.Controllers
{
    [Route("/api")]
    [Authorize]
    public class ApiController : Controller
    {
        private readonly IWordsService wordsService;
        private readonly IStatisticsService statisticsService;

        public ApiController(IWordsService wordsService, IStatisticsService statisticsService)
        {
            this.wordsService = wordsService;
            this.statisticsService = statisticsService;
        }

        [HttpPost]
        [Route("words")]
        public async Task<IActionResult> AddWord([Required][FromBody] AddWordInputModel input)
        {
            AddWordResult result = null;

            try
            {
                var dbWord = await this.wordsService.AddWordAsync(input?.Word);
                result = new AddWordResult(dbWord.Id);

                await this.statisticsService.AddWordAdditionAsync(
                                                word: input?.Word,
                                                wordId: result.Id,
                                                hasError: false,
                                                errorMessage: null,
                                                ipAddress: this.HttpContext.Connection.RemoteIpAddress.ToString());
                return this.Json(result);
            }
            catch (AddWordException ex)
            {
                result = new AddWordResult(ex.Message, ErrorSeverity.Warning);

                await this.statisticsService.AddWordAdditionAsync(
                                                word: input?.Word,
                                                wordId: null,
                                                hasError: true,
                                                errorMessage: ex.Message,
                                                ipAddress: this.HttpContext.Connection.RemoteIpAddress.ToString());
                return this.BadRequest(result);
            }
            catch (Exception ex)
            {
                result = new AddWordResult(ex.Message, ErrorSeverity.Error);

                await this.statisticsService.AddWordAdditionAsync(
                                                word: input?.Word,
                                                wordId: null,
                                                hasError: true,
                                                errorMessage: ex.Message,
                                                ipAddress: this.HttpContext.Connection.RemoteIpAddress.ToString());
                return this.BadRequest(result);
            }
        }
    }
}