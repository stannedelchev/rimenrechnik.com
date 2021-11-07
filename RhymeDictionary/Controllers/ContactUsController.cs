using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RhymeDictionary.Helpers;
using RhymeDictionary.Model.Services;

namespace RhymeDictionary.Controllers
{
    [Route("контакти")]
    public class ContactUsController : Controller
    {
        private readonly ISiteCommentsService commentsService;

        public ContactUsController(ISiteCommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            this.ViewData[Constants.ActivePageViewDataId] = Constants.ContactUsNavbarId;
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(CommentViewModel comment)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View("Index", comment);
            }

            await this.commentsService.AddCommentAsync(comment.Email, comment.Text);

            return this.RedirectToAction("благодарим-ви", "контакти");
        }

        [Route("благодарим-ви")]
        public IActionResult ThankYou()
        {
            return this.View();
        }
    }

#pragma warning disable SA1402 // File may only contain a single class
    public class CommentViewModel
#pragma warning restore SA1402 // File may only contain a single class
    {
        [Required(ErrorMessage = "Моля въведете имейл за обратна връзка")]
        [EmailAddress(ErrorMessage = "Моля въведете правилен имейл за обратна връзка")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Моля въведете коментар")]
        [MaxLength(4000, ErrorMessage = "Моля въведете по-кратък коментар")]
        public string Text { get; set; }
    }
}