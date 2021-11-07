//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Threading.Tasks;
//using Dapper;
//using RhymeDictionary.Model.DbEntities;

//namespace RhymeDictionary.Model.Services
//{
//    [Obsolete("Use InMemoryRhymesService")]
//    public class SqlServerDbRhymesService : IRhymesService
//    {
//        private readonly string connectionString;

//        public SqlServerDbRhymesService(string connectionString)
//        {
//            this.connectionString = connectionString;
//        }

//        public async Task<IEnumerable<WordEntity>> GetRhymesAsync(string word)
//        {
//            using (var connection = new SqlConnection(this.connectionString))
//            {
//                var res = await connection.QueryAsync<WordEntity>(
//@"DECLARE @reverseEnding nvarchar(400)
//SELECT @reverseEnding = ReverseEnding FROM [words] WHERE normal = @Word
//SELECT id, Normal, Forward, Reverse, ReverseEnding FROM words 
//    WHERE [Reverse] LIKE CONCAT(ISNULL(@reverseEnding, '-----NO ENDING FOUND-----') , '%') 
//    AND normal NOT LIKE @Word;",
//new { Word = word });
//                return res;
//            }
//        }
//    }
//}