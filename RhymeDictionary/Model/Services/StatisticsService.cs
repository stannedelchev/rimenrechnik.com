using System;
using System.Threading.Tasks;
using Dapper.FastCrud;
using Npgsql;
using RhymeDictionary.Model.DbEntities;

namespace RhymeDictionary.Model.Services
{
    internal class StatisticsService : IStatisticsService
    {
        private readonly string connectionString;

        public StatisticsService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task AddSearchVisitAsync(string word, string ipAddress)
        {
            var visit = new WordSearchVisitEntity(Guid.NewGuid(), word, DateTime.UtcNow, ipAddress);
            using (var connection = new NpgsqlConnection(this.connectionString))
            {
                await connection.InsertAsync(visit);
            }
        }

        public Task AddWordAdditionAsync(string word, Guid? wordId, bool hasError, string errorMessage, string ipAddress)
        {
            throw new NotImplementedException();
            //var addition = new WordAdditionEntity(Guid.NewGuid(), word, wordId, hasError, errorMessage, DateTime.UtcNow, ipAddress);

            //using (var connection = new NpgsqlConnection(this.connectionString))
            //{
            //    await connection.ExecuteAsync("INSERT INTO WordAdditions(Id, Word, WordId, HasError, ErrorMessage, DateAddedUtc, IpAddress) VALUES(@Id, @Word, @WordId, @HasError, @ErrorMessage, @DateAddedUtc, @IpAddress)", addition);
            //}
        }
    }
}