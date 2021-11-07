using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RhymeDictionary.Model.DbEntities
{
    [Table("WordSuggestions")]
    public class WordSuggestionEntity
    {
        public Guid Id { get; set; }

        public string Word { get; set; }

        public string IpAddress { get; set; }

        public DateTime DateAddedUtc { get; set; }
    }
}
