using System.Linq;
using RhymeDictionary.Model.Extensions;

namespace RhymeDictionary.Model
{
    public class SyllableCounter
    {
        public int Count(string word) => word.Count(c => CharExtensions.IsBulgarianVowel(c));
    }
}