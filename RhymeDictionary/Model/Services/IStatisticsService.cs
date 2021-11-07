using System;
using System.Threading.Tasks;

namespace RhymeDictionary.Model.Services
{
    public interface IStatisticsService
    {
        Task AddSearchVisitAsync(string word, string ipAddress);

        Task AddWordAdditionAsync(string word, Guid? wordId, bool hasError, string errorMessage, string ipAddress);
    }
}