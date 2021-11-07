using System.Threading.Tasks;

namespace RhymeDictionary.Model.Services
{
    public interface ISiteCommentsService
    {
        Task AddCommentAsync(string email, string text);
    }
}
