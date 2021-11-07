using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RhymeDictionary.Model.DbEntities;

namespace RhymeDictionary.Migration.Chitanka
{
    public interface IWordsService
    {
        Task AddRangeAsync(IEnumerable<WordEntity> words);

        Task RemoveAllAsync();

        Task<IEnumerable<WordEntity>> GetAllWordsAsync();

        Task<WordEntity> AddWordAsync(string word);
    }

    public class ChitankaMigrator
    {
        private readonly ChitankaRechkoService chitankaService;
        private readonly IWordsService wordsService;

        public ChitankaMigrator(ChitankaRechkoService chitankaService, IWordsService wordsService)
        {
            this.chitankaService = chitankaService;
            this.wordsService = wordsService;
        }

        public async Task MigrateAsync()
        {
            var chitankaTask = this.chitankaService.GetCorrectlyStressedWordsAsync();
            var modelTask = this.wordsService.GetAllWordsAsync();

            await Task.WhenAll(chitankaTask, modelTask);

            var chitankaWords = chitankaTask.Result.Select(w => w.ToModelWord()).ToList();

            var modelWordsCache = modelTask.Result.ToDictionary(w => w.Normal.ToLower(), w => w);
            var chitankaWordsCache = chitankaWords.ToDictionary(w => w.Normal.ToLower(), w => w);

            var wordsToBeImported = chitankaWordsCache.Where(w => !modelWordsCache.ContainsKey(w.Key.ToLower())).Select(kvp => kvp.Value).ToList();

            await this.wordsService.AddRangeAsync(wordsToBeImported);
        }
    }
}