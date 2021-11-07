using System;
using System.Threading.Tasks;
using Dapper.FastCrud;
using Npgsql;
using RhymeDictionary.Model.DbEntities;

namespace RhymeDictionary.Model.Services
{
    internal class WordSuggestionsService : IWordSuggestionsService
    {
        private readonly string connectionString;

        public WordSuggestionsService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task AddSuggestions(string suggestions, string ipAddress)
        {
            if (string.IsNullOrEmpty(suggestions))
            {
                return;
            }

            var now = DateTime.UtcNow;
            var suggestionsEntity = new WordSuggestionEntity()
            {
                Id = Guid.NewGuid(),
                IpAddress = ipAddress,
                DateAddedUtc = now,
                Word = suggestions
            };

            using (var connection = new NpgsqlConnection(this.connectionString))
            {
                await connection.OpenAsync();
                await connection.InsertAsync(suggestionsEntity);
            }
        }
    }
}
