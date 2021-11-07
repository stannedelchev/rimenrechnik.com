using System.Threading.Tasks;
using Prism.Mvvm;
using RhymeDictionary.Wpf.Services;

namespace RhymeDictionary.Wpf
{
    public enum SentWordViewModelState
    {
        NotSet = 0,
        Loading = 1,
        FinishedOk = 2,
        FinishedError = 3,
        FinishedWarning = 4
    }
}