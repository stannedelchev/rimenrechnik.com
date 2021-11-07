using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RhymeDictionary.Model;
using RhymeDictionary.Model.DbEntities;

namespace RhymeDictionary.ViewModels
{
    public class RhymesViewModel
    {
        public RhymesViewModel(string originalWord, IEnumerable<WordEntity> rhymes)
        {
            this.Word = originalWord;
            this.Count = rhymes?.Count() ?? 0;
            this.Monosyllabic = rhymes.Where(w => w.Syllables == 1);
            this.Dissyllabic = rhymes.Where(w => w.Syllables == 2);
            this.Trisyllabic = rhymes.Where(w => w.Syllables == 3);
            this.Polysyllabic = rhymes.Where(w => w.Syllables >= 4);
            this.All = rhymes;
        }

        public string Word { get; private set; }

        public int Count { get; private set; }

        public IEnumerable<WordEntity> All { get; private set; }

        public IEnumerable<WordEntity> Dissyllabic { get; private set; }

        public IEnumerable<WordEntity> Monosyllabic { get; private set; }

        public IEnumerable<WordEntity> Polysyllabic { get; private set; }

        public IEnumerable<WordEntity> Trisyllabic { get; private set; }
    }
}
