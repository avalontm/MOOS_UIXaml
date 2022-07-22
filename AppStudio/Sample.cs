using AppStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;

namespace XamlMOOS
{
    public class Sample
    {
        public Sample()
        { 
        
        }

        public Window Get()
        {
            Window window = new Window();
            window.Title = "MOOS GUI";
            window.Width = 300;
            window.Height = 300;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Grid grid = new Grid();
            Button button = new Button();
            button.Command = new RelayCommand(ButtonCommand);
            button.Background = new SolidColorBrush(Color.FromArgb(BitConverter.GetBytes(14606046)[3], BitConverter.GetBytes(14606046)[2], BitConverter.GetBytes(14606046)[1], BitConverter.GetBytes(14606046)[0]));
            button.Margin = new Thickness(5);
            button.Content = "Click Me!";
            grid.Children.Add(button);
            window.Content = grid;
            return window;
        }

        void ButtonCommand(object obj)
        {
            MessageBox.Show("Click!");
        }
    }

    public class RelayCommand : ICommand
    {

        private Action<object> mAction;

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public RelayCommand(Action<object> action)
        {
            mAction = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            mAction(parameter);
        }
    }
}
