using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RhymeDictionary.ViewModels
{
    public class AddWordsViewModel
    {
        [Required]
        [StringLength(10000)]
        public string Suggestions { get; set; }

        public bool? Result { get; set; }

        public string ErrorMessage { get; set; }
    }
}
