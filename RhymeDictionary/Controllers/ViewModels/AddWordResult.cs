using System;

namespace RhymeDictionary.Controllers.ViewModels
{
    public class AddWordResult
    {
        public AddWordResult(Guid id)
        {
            this.Id = id;
        }

        public AddWordResult(string errorMessage, ErrorSeverity severity)
        {
            this.HasError = true;
            this.ErrorSeverity = severity;
            this.ErrorMessage = errorMessage;
        }

        public bool HasError { get; }

        public ErrorSeverity ErrorSeverity { get; }

        public string ErrorMessage { get; }

        public string Word { get; set; }

        public Guid? Id { get; }
    }
}