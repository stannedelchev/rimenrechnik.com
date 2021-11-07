using System;

namespace RhymeDictionary.Wpf.Services
{
    public class AddWordResult
    {
        public bool HasError { get; set; }

        public ErrorSeverity ErrorSeverity { get; set; }

        public string ErrorMessage { get; set; }

        public string Word { get; set; }

        public Guid? Id { get; set; }
    }
}
