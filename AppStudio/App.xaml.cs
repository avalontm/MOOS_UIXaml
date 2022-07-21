using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace AppStudio
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Get().ShowDialog();

        }

        private Window Get()
        {
            Window window = new Window();
            window.Title = "MOOS GUI";
            window.Width = 300D;
            window.Height = 300D;
            window.WindowStartupLocation = ((WindowStartupLocation)(TypeDescriptor.GetConverter(typeof(WindowStartupLocation)).ConvertFromInvariantString("CenterScreen")));
            Grid grid = new Grid();
            Button button = new Button();
            button.Command = new RelayCommand(ButtonCommand);
            button.Margin = ((Thickness)(TypeDescriptor.GetConverter(typeof(Thickness)).ConvertFromInvariantString("5")));
            button.Content = "Click Me!";
            grid.Children.Add(button);
            window.Content = grid;
            return window;
        }


        void ButtonCommand(object obj)
        {
            MessageBox.Show("Clicked!");
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

