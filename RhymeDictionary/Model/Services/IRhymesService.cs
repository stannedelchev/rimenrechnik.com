using System.Collections.Generic;
using System.Threading.Tasks;
using RhymeDictionary.Model.DbEntities;

namespace RhymeDictionary.Model.Services
{
    public interface IRhymesService
    {
        Task<IEnumerable<WordEntity>> GetRhymesAsync(string word);
    }
}