using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace RhymeDictionary.Wpf.Converters
{
    internal class SentWordStateToBackgroundConverter : IValueConverter
    {
        public Brush NotSet { get; set; }

        public Brush Loading { get; set; }

        public Brush FinishedOk { get; set; }

        public Brush FinishedError { get; set; }

        public Brush FinishedWarning { get; set; }

        public Brush Default { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var severity = (SentWordViewModelState)value;
            switch (severity)
            {
                case SentWordViewModelState.NotSet:
                    return this.NotSet;
                case SentWordViewModelState.Loading:
                    return this.Loading;
                case SentWordViewModelState.FinishedOk:
                    return this.FinishedOk;
                case SentWordViewModelState.FinishedError:
                    return this.FinishedError;
                case SentWordViewModelState.FinishedWarning:
                    return this.FinishedWarning;
                default:
                    return this.Default;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
