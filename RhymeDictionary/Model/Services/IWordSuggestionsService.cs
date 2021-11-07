using System.Threading.Tasks;

namespace RhymeDictionary.Model.Services
{
    public interface IWordSuggestionsService
    {
        Task AddSuggestions(string words, string ipAddress);
    }
}
