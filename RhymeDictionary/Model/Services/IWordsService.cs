using System.Collections.Generic;
using System.Threading.Tasks;
using RhymeDictionary.Model.DbEntities;

namespace RhymeDictionary.Model.Services
{
    public interface IWordsService
    {
        Task<IEnumerable<WordEntity>> GetAllWordsAsync();

        Task<WordEntity> AddWordAsync(string word);
    }
}