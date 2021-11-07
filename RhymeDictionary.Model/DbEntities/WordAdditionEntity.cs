using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhymeDictionary.Model.DbEntities
{
    [Table("WordAdditions")]
    public class WordAdditionEntity
    {
        public WordAdditionEntity(Guid id, string word, Guid? wordId, bool hasError, string errorMessage, DateTime dateAddedUtc, string ipAddress)
        {
            this.Id = id;
            this.Word = word;
            this.WordId = wordId;
            this.HasError = hasError;
            this.ErrorMessage = errorMessage;
            this.DateAddedUtc = dateAddedUtc;
            this.IpAddress = ipAddress;
        }

        public Guid Id { get; }

        public string Word { get; }

        public Guid? WordId { get; set; }

        public bool HasError { get; }

        public string ErrorMessage { get; }

        public DateTime DateAddedUtc { get; }

        public string IpAddress { get; }
    }
}