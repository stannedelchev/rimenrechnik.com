using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace RhymeDictionary.Wpf.Behaviors
{
    internal class SendCommandAndClearTextOnEnterKeyAction : TriggerAction<TextBox>
    {
        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(SendCommandAndClearTextOnEnterKeyAction), new PropertyMetadata(null));

        public ICommand Command
        {
            get { return (ICommand)this.GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }

        protected override void Invoke(object parameter)
        {
            var args = parameter as KeyEventArgs;
            if (args?.Key == Key.Enter)
            {
                if (this.Command.CanExecute(this.AssociatedObject.Text))
                {
                    this.Command?.Execute(this.AssociatedObject.Text);
                    this.AssociatedObject.ClearValue(TextBox.TextProperty);
                }
            }
        }
    }
}
