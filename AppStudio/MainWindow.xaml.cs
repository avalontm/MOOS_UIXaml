using GUIStudio;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XamlConversion;
using XamlMOOS;

namespace AppStudio
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        string _xamlCode;
        public string xamlCode
        {
            get { return _xamlCode; }
            set
            {
                _xamlCode = value;
                RaisePropertyChanged("xamlCode");
            }
        }

        string _debug;
        public string debug
        {
            get { return _debug; }
            set
            {
                _debug = value;
                RaisePropertyChanged("debug");
            }
        }

        XamlConvertor _xamlConvert;
        public XamlConvertor xamlConvert
        {
            get { return _xamlConvert; }
            set
            {
                _xamlConvert = value;
                RaisePropertyChanged("xamlConvert");
            }
        }

        public static MainWindow Instance { private set; get; }

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            xamlConvert = new XamlConvertor();
            xamlCode = "<Window Title=\"MOOS GUI\" Width=\"300\" Height=\"300\" WindowStartupLocation=\"CenterScreen\">\n";
            xamlCode += "   <Window.Content>\n";
            xamlCode += "       <Grid>\n";
            xamlCode += "           <Grid.Children>\n";
            xamlCode += "               <Button Command=\"{Binding ButtonCommand}\" Background=\"#dedede\" Margin=\"5\" Content=\"Click Me!\"/>\n";
            xamlCode += "           </Grid.Children>\n";
            xamlCode += "       </Grid>\n";
            xamlCode += "   </Window.Content>\n";
            xamlCode += "</Window>\n";
            txtEditor.Text = xamlCode;
            DataContext = this;
        }

        void onLoaded(object sender, RoutedEventArgs e)
        {
            Instance = this;
        }

        void onUnloaded(object sender, RoutedEventArgs e)
        {
            Instance = null;
        }

        async void onCompile(object sender, RoutedEventArgs e)
        {
            string result = xamlConvert.ConvertToString(txtEditor.Text);
            debug = result + "\n";
            debug += "==================== [Finish] ====================" + "\n\n";
        }

        void onChangedFontSize(object sender, SelectionChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbFontSize.Text))
            {
                string content = ((ComboBoxItem)cmbFontSize.Items[cmbFontSize.SelectedIndex]).Content.ToString();
                txtEditor.FontSize = ((double.Parse(content) * 96) / 72);
            }
        }

        void onNew(object sender, RoutedEventArgs e)
        {
            var result =  MessageBox.Show("Are you sure you want to create a new document?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                return;
            }

            txtEditor.Text = string.Empty;
        }

        void onOpen(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Moos Xaml File (*.mxaml)|*.mxaml";

            if (openFileDialog.ShowDialog() == true)
{
                txtEditor.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }

        void onSave(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtEditor.Text))
            {
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Moos Xaml File (*.mxaml)|*.mxaml";
            saveFileDialog.FileName = "moos_gui";

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, txtEditor.Text);
            }

        }


    }
}
