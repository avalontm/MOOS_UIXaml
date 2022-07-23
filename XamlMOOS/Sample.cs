﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;



public partial class MOOS : System.Windows.Window
{

    private System.Globalization.CultureInfo EnglishCultureInfo = new System.Globalization.CultureInfo("en-us", false);

    public MOOS()
    {
        ClickMeCommand = new RelayCommand(onClick);

        this.Title = "MOOS GUI";
        this.Width = 300;
        this.Height = 300;
        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        // ---------------------------
        Grid _grid0 = new Grid();
        this.Content = _grid0;
        // ---------------------------
        RowDefinitionCollection _rowDefinitionCollection1 = _grid0.RowDefinitions;
        // ---------------------------
        RowDefinition _rowDefinition2 = new RowDefinition();
        _rowDefinitionCollection1.Add(_rowDefinition2);
        XamlMOOS.Conveters.GridLengthConverter _gridLengthConverter = new XamlMOOS.Conveters.GridLengthConverter();
        _rowDefinition2.Height = ((GridLength)(_gridLengthConverter.ConvertFrom(null, EnglishCultureInfo, "Auto")));
        // ---------------------------
        RowDefinition _rowDefinition3 = new RowDefinition();
        _rowDefinitionCollection1.Add(_rowDefinition3);
        _rowDefinition3.Height = ((GridLength)(_gridLengthConverter.ConvertFrom(null, EnglishCultureInfo, "*")));
        // ---------------------------
        RowDefinition _rowDefinition4 = new RowDefinition();
        _rowDefinitionCollection1.Add(_rowDefinition4);
        _rowDefinition4.Height = ((GridLength)(_gridLengthConverter.ConvertFrom(null, EnglishCultureInfo, "10")));
        // ---------------------------
        UIElementCollection _uIElementCollection5 = _grid0.Children;
        // ---------------------------
        Button _button6 = new Button();
        _uIElementCollection5.Add(_button6);
        Grid.SetRow(_button6, 0);
        // ---------------------------
        Binding _binding7 = new Binding("");
        _binding7.Source = ClickMeCommand;
        _button6.SetBinding(System.Windows.Controls.Button.CommandProperty, _binding7);
        XamlMOOS.Conveters.BrushConverter _brushConverter = new XamlMOOS.Conveters.BrushConverter();
        _button6.Background = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#dedede")));
        XamlMOOS.Conveters.ThicknessConverter _thicknessConverter = new XamlMOOS.Conveters.ThicknessConverter();
        _button6.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button6.Content = "Click Me!";
        // ---------------------------
        Button _button8 = new Button();
        _uIElementCollection5.Add(_button8);
        Grid.SetRow(_button8, 1);
        // ---------------------------
        Binding _binding9 = new Binding("");
        _binding9.Source = ClickMeCommand;
        _button8.SetBinding(System.Windows.Controls.Button.CommandProperty, _binding9);
        _button8.Background = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#dedede")));
        _button8.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button8.Content = "Click Me!";
    }

    // ---------------------------  
    void onClick(object obj)
    {
        MessageBox.Show("Clicked!");
    }

    ICommand ClickMeCommand { get; set; }
    public class RelayCommand : ICommand
    {
        readonly Predicate<object> _canExecute;
        readonly Action<object> _execute;

        public RelayCommand(Action<object> execute)
        {
            _execute = execute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}