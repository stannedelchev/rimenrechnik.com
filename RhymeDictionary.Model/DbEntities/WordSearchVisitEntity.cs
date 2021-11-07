using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhymeDictionary.Model.DbEntities
{
    [Table("WordSearchVisits")]
    public class WordSearchVisitEntity
    {
        public WordSearchVisitEntity(Guid id, string word, DateTime dateSearchedUtc, string ipAddress)
        {
            this.Id = id;
            this.Word = word ?? string.Empty;
            this.DateSearchedUtc = dateSearchedUtc;
            this.IpAddress = ipAddress;
        }

        public Guid Id { get; private set; }

        public string Word { get; private set; }

        public DateTime DateSearchedUtc { get; private set; }

        public string IpAddress { get; private set; }
    }
}