using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using RhymeDictionary.Model.DbEntities;

namespace RhymeDictionary.Model.Chitanka
{
    [Table("word")]
    [DebuggerDisplay("{Name_Stressed}: {Name}")]
    public class ChitankaWord
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Name_Stressed { get; set; }

        public string Name_Broken { get; set; }

        public string Name_Condensed { get; set; }

        public string Meaning { get; set; }

        public string Synonyms { get; set; }

        public string Classification { get; set; }

        public string Type_Id { get; set; }

        public string Pronounciation { get; set; }

        public string Etymology { get; set; }

        public string Related_Words { get; set; }

        public string Derived_Words { get; set; }

        public string Chitanka_Count { get; set; }

        public string Chitanka_Percent { get; set; }

        public string Chitanka_Rank { get; set; }

        public string Search_Count { get; set; }

        public string Source { get; set; }

        public string Other_Langs { get; set; }

        public string Deleted_At { get; set; }

        public string Corpus_Count { get; set; }

        public string Corpus_Percent { get; set; }

        public string Corpus_Rank { get; set; }

        public bool IsCorrectlyStressed
        {
            get
            {
                var syllables = new SyllableCounter().Count(this.Name);
                var isSingleSyllable = syllables <= 1;

                return isSingleSyllable || this.Name_Stressed.Contains("`");
            }
        }

        public WordEntity ToModelWord()
        {
            string forward = this.ReplaceChitankaStressWithModelStress();
            var word = new WordEntity()
            {
                Normal = this.Name.ToLower(),
                Id = Guid.NewGuid(),
                Forward = forward,
                Reverse = new string(forward.ToCharArray().Reverse().ToArray())
            };

            return word;
        }

        public string ReplaceChitankaStressWithModelStress()
        {
            if (string.IsNullOrEmpty(this.Name_Stressed) || !this.IsCorrectlyStressed)
            {
                return this.Name_Stressed;
            }

            var result = this.Name_Stressed.ToLower()
                .Replace("а`", "'а")
                .Replace("ъ`", "'ъ")
                .Replace("о`", "'о")
                .Replace("у`", "'у")
                .Replace("е`", "'е")
                .Replace("и`", "'и")
                .Replace("ю`", "'ю")
                .Replace("я`", "'я");

            if (new SyllableCounter().Count(this.Name) == 1)
            {
                result = result
                        .Replace("а", "'а")
                        .Replace("ъ", "'ъ")
                        .Replace("о", "'о")
                        .Replace("у", "'у")
                        .Replace("е", "'е")
                        .Replace("и", "'и")
                        .Replace("ю", "'ю")
                        .Replace("я", "'я");
            }

            return result;
        }

        public class DefaultComparer : IEqualityComparer<ChitankaWord>
        {
            public bool Equals(ChitankaWord x, ChitankaWord y)
            {
                return string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
            }

            public int GetHashCode(ChitankaWord obj)
            {
                return obj.Name.ToLower().GetHashCode();
            }
        }
    }
}