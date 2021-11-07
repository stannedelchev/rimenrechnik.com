using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper.FastCrud;
using Npgsql;
using RhymeDictionary.Model.DbEntities;

namespace RhymeDictionary.Model.Services
{
    internal class WordsService : IWordsService
    {
        private readonly string connectionString;

        static WordsService()
        {
            OrmConfiguration.GetDefaultEntityMapping<WordEntity>().RemoveProperty(w => w.Ending);
        }

        public WordsService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<IEnumerable<WordEntity>> GetAllWordsAsync()
        {
            using (var connection = new NpgsqlConnection(this.connectionString))
            {
                var words = await connection.FindAsync<WordEntity>();
                return words;
            }
        }

        public Task<WordEntity> AddWordAsync(string word)
        {
            throw new NotImplementedException();
            //if (string.IsNullOrEmpty(word) || string.IsNullOrWhiteSpace(word))
            //{
            //    throw new ArgumentException("Word should be entered");
            //}

            //bool wordContainsStress = word.ContainsStress();
            //bool wordContainsUpperCaseVowel = word.ContainsUpperCaseVowel();
            //bool wordContainsBothTypesOfStress = wordContainsStress && wordContainsUpperCaseVowel;

            //if ((!wordContainsStress && !wordContainsUpperCaseVowel)
            //    || wordContainsBothTypesOfStress)
            //{
            //    throw new ArgumentException("Word should contain either stress or an uppercase vowel");
            //}

            //string wordWithStress = word.CoerceToLowerCaseStressed();

            //var dbEntity = WordEntity.FromForward(wordWithStress);

            //if (dbEntity?.Id == null)
            //{
            //    throw new InvalidOperationException("Database word entity should have a GUID.");
            //}

            //if (string.IsNullOrEmpty(dbEntity.Normal)
            //    || string.IsNullOrEmpty(dbEntity.Forward)
            //    || string.IsNullOrEmpty(dbEntity.Reverse)
            //    || string.IsNullOrEmpty(dbEntity.ReverseEnding))
            //{
            //    throw new ArgumentException("Database entity should be a valid word");
            //}

            //using (var connection = new NpgsqlConnection(this.connectionString))
            //{
            //    var existingWords = await connection.FindAsync<WordEntity>(statement => statement
            //                                                                                .Where($"{nameof(WordEntity.Forward):C}=@Forward")
            //                                                                                .WithParameters(new { Forward = dbEntity.Forward }));
            //    if (existingWords.AsList().Count != 0)
            //    {
            //        throw new AddWordException("Word already exists");
            //    }

            //    await connection.InsertAsync(dbEntity);
            //}

            //return dbEntity;
        }
    }
}
