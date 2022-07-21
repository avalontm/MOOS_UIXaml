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

        public MainWindow()
        {
            InitializeComponent();
            xamlConvert = new XamlConvertor();
            xamlCode = "<Window Title=\"MOOS GUI\" Width=\"300\" Height=\"300\" WindowStartupLocation=\"CenterScreen\">\n";
            xamlCode += "   <Window.Content>\n";
            xamlCode += "       <Grid>\n";
            xamlCode += "           <Grid.Children>\n";
            xamlCode += "               <Button Command=\"{Binding ButtonCommand}\" Margin=\"5\" Content=\"Click Me!\"/>\n";
            xamlCode += "           </Grid.Children>\n";
            xamlCode += "       </Grid>\n";
            xamlCode += "   </Window.Content>\n";
            xamlCode += "</Window>\n";
            txtEditor.Text = xamlCode;
            DataContext = this;
        }


        void onCompile(object sender, RoutedEventArgs e)
        {
            string result = xamlConvert.ConvertToString(txtEditor.Text);
            Debug.WriteLine(result);

            if (string.IsNullOrEmpty(result))
            {
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "C# file (*.cs)|*.cs";

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, result);
            }
        }
    }
}
