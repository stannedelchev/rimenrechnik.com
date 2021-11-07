using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhymeDictionary.Wpf.Services
{
    public class AddWordInputModel
    {
        public AddWordInputModel(string word)
        {
            this.Word = word;
        }

        public string Word { get; set; }
    }
}
