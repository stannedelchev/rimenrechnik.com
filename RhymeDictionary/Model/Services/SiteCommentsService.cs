using System;
using System.Threading.Tasks;
using Dapper;
using Dapper.FastCrud;
using Npgsql;
using RhymeDictionary.Model.DbEntities;

namespace RhymeDictionary.Model.Services
{
    internal class SiteCommentsService : ISiteCommentsService
    {
        private readonly string connectionString;

        public SiteCommentsService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task AddCommentAsync(string email, string text)
        {
            using (var connection = new NpgsqlConnection(this.connectionString))
            {
                var comment = new SiteCommentEntity(Guid.NewGuid(), email, text, DateTime.UtcNow);
                await connection.InsertAsync(comment);
            }
        }
    }
}
