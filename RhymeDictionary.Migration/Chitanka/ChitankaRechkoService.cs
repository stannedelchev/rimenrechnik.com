using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper.FastCrud;
using MySql.Data.MySqlClient;
using RhymeDictionary.Model.Chitanka;

namespace RhymeDictionary.Migration.Chitanka
{
    public class ChitankaRechkoService
    {
        private readonly string connectionString;

        public ChitankaRechkoService(string connectionString)
        {
            this.connectionString = connectionString;

            OrmConfiguration.GetDefaultEntityMapping<ChitankaWord>().SetDialect(SqlDialect.MySql);
        }

        public async Task<IEnumerable<ChitankaWord>> GetCorrectlyStressedWordsAsync()
        {
            using (var connection = new MySqlConnection(this.connectionString))
            {
                var allChitankaWords = await connection.FindAsync<ChitankaWord>(stmnt => stmnt.Where($"{nameof(ChitankaWord.Name_Stressed):C} IS NOT NULL"));
                var chitankaWords = allChitankaWords.Where(w => w.IsCorrectlyStressed).Distinct(new ChitankaWord.DefaultComparer()).ToList();
                return chitankaWords;
            }
        }
    }
}
