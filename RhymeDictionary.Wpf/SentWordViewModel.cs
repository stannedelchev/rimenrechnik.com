using System;
using System.Threading.Tasks;
using Prism.Mvvm;
using RhymeDictionary.Wpf.Services;

namespace RhymeDictionary.Wpf
{
    public class SentWordViewModel : BindableBase
    {
        private string errorMessage;
        private SentWordViewModelState state;

        public SentWordViewModel(string word)
        {
            this.Word = word;
        }

        public string Word { get; }

        public SentWordViewModelState State
        {
            get { return this.state; }
            set { this.SetProperty(ref this.state, value); }
        }

        public string ErrorMessage
        {
            get { return this.errorMessage; }
            set { this.SetProperty(ref this.errorMessage, value); }
        }

        public async Task SendAsync()
        {
            try
            {
                this.State = SentWordViewModelState.Loading;

                var baseApiUrl = ApplicationWideSettings.ApiUrl;
                var baseTokenUrl = ApplicationWideSettings.TokenDiscoveryUrl;
                var result = await new RimenRechnikApi(baseApiUrl, baseTokenUrl).AddWordAsync(this.Word);

                this.State = result.HasError
                                ? result.ErrorSeverity == ErrorSeverity.Error
                                    ? SentWordViewModelState.FinishedError
                                    : result.ErrorSeverity == ErrorSeverity.Warning
                                        ? SentWordViewModelState.FinishedWarning
                                        : throw new InvalidOperationException()
                                : SentWordViewModelState.FinishedOk;

                this.ErrorMessage = result.ErrorMessage;
            }
            catch (Exception ex)
            {
                this.State = SentWordViewModelState.FinishedError;
                this.ErrorMessage = ex.Message;
            }
        }
    }
}