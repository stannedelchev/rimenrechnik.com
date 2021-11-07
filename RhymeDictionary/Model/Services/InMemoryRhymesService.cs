using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RhymeDictionary.Model.DbEntities;

namespace RhymeDictionary.Model.Services
{
    public class InMemoryRhymesService : IRhymesService
    {
        private static readonly SemaphoreSlim InitializeLock = new SemaphoreSlim(1);

        private readonly IWordsService wordsService;
        private readonly SortedDictionary<string, WordEntity> normalWords = new SortedDictionary<string, WordEntity>();
        private readonly SortedDictionary<string, List<WordEntity>> wordsByReverseEnding = new SortedDictionary<string, List<WordEntity>>();

        private bool isInitialized;


        public InMemoryRhymesService(IWordsService wordsService)
        {
            this.wordsService = wordsService;
        }

        public async Task<IEnumerable<WordEntity>> GetRhymesAsync(string word)
        {
            try
            {
                await InitializeLock.WaitAsync();
                if (!this.isInitialized)
                {
                    await this.InitializeAsync();
                    this.isInitialized = true;
                }
            }
            finally
            {
                InitializeLock.Release();
            }

            return !this.normalWords.TryGetValue(word, out var wordEntity)
                ? Enumerable.Empty<WordEntity>()
                : this.wordsByReverseEnding[wordEntity.ReverseEnding];
        }

        private async Task InitializeAsync()
        {
            var words = (await this.wordsService.GetAllWordsAsync()).ToList();
            foreach (var word in words)
            {
                this.normalWords.Add(word.Normal, word);

                try
                {
                    if (!this.wordsByReverseEnding.ContainsKey(word.ReverseEnding))
                    {
                        this.wordsByReverseEnding.Add(word.ReverseEnding, new List<WordEntity>());
                        continue;
                    }

                    this.wordsByReverseEnding[word.ReverseEnding].Add(word);
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}