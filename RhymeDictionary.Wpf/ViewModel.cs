using System.Collections.ObjectModel;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;

namespace RhymeDictionary.Wpf
{
    internal class ViewModel : BindableBase
    {
        private string word;
        private string serverUrl;

        public ViewModel()
        {
            this.SentWords = new ObservableCollection<SentWordViewModel>();
            this.AddWordCommand = new DelegateCommand<string>(this.OnAddWordCommandExecuted, this.CanAddWordCommandExecute)
                .ObservesProperty(() => this.Word);
        }

        public ICommand AddWordCommand { get; }

        public ObservableCollection<SentWordViewModel> SentWords { get; }

        public string Word
        {
            get { return this.word; }
            set { this.SetProperty(ref this.word, value); }
        }

        public string ServerUrl
        {
            get
            {
                return ApplicationWideSettings.ApiUrl;
            }

            set
            {
                ApplicationWideSettings.ApiUrl = value;
                this.RaisePropertyChanged();
            }
        }

        public string ServerTokenUrl
        {
            get
            {
                return ApplicationWideSettings.TokenDiscoveryUrl;
            }

            set
            {
                ApplicationWideSettings.TokenDiscoveryUrl = value;
                this.RaisePropertyChanged();
            }
        }

        public string ServerClientId
        {
            get
            {
                return ApplicationWideSettings.ClientId;
            }

            set
            {
                ApplicationWideSettings.ClientId = value;
                this.RaisePropertyChanged();
            }
        }

        public string ServerClientSecret
        {
            get
            {
                return ApplicationWideSettings.ClientSecret;
            }

            set
            {
                ApplicationWideSettings.ClientSecret = value;
                this.RaisePropertyChanged();
            }
        }

        private bool CanAddWordCommandExecute(string arg)
        {
            return !string.IsNullOrEmpty(arg) && !string.IsNullOrWhiteSpace(arg);
        }

        private async void OnAddWordCommandExecuted(string word)
        {
            var vm = new SentWordViewModel(word);
            this.SentWords.Insert(0, vm);
            await vm.SendAsync();
        }
    }
}
