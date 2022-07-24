﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;


public partial class UIMoos : Window
{
    CultureInfo EnglishCultureInfo = new CultureInfo("en-us", false);
    ICommand ClickMeCommand { get; set; }
    ICommand ClickEnterCommand { get; set; }
    ICommand ClickDelCommand { get; set; }
    ICommand ClickMinusCommand { get; set; }
    ICommand ClickPlusCommand { get; set; }

    public System.Windows.Controls.Label lblNumbers;

    Opreation opreation = Opreation.None;

    enum Opreation
    {
        None,
        Plus,
        Minus
    }

    ulong Num1 = 0;
    ulong Num2 = 0;

    public UIMoos()
    {
        ClickMeCommand = new ICommand(onClick);
        ClickEnterCommand = new ICommand(onEnter);
        ClickDelCommand = new ICommand(onDelete);
        ClickPlusCommand = new ICommand(onPlus);
        ClickMinusCommand = new ICommand(onMinus);

        this.Title = "MOOS GUI";
        this.Width = 300;
        this.Height = 350;
        this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        // ---------------------------
        Grid _grid0 = new Grid();
        this.Content = _grid0;
        // ---------------------------
        RowDefinitionCollection _rowDefinitionCollection1 = _grid0.RowDefinitions;
        // ---------------------------
        RowDefinition _rowDefinition2 = new RowDefinition();
        _rowDefinitionCollection1.Add(_rowDefinition2);
        GridLengthConverter _gridLengthConverter = new GridLengthConverter();
        _rowDefinition2.Height = ((GridLength)(_gridLengthConverter.ConvertFrom(null, EnglishCultureInfo, "50")));
        // ---------------------------
        RowDefinition _rowDefinition3 = new RowDefinition();
        _rowDefinitionCollection1.Add(_rowDefinition3);
        _rowDefinition3.Height = ((GridLength)(_gridLengthConverter.ConvertFrom(null, EnglishCultureInfo, "*")));
        // ---------------------------
        RowDefinition _rowDefinition4 = new RowDefinition();
        _rowDefinitionCollection1.Add(_rowDefinition4);
        _rowDefinition4.Height = ((GridLength)(_gridLengthConverter.ConvertFrom(null, EnglishCultureInfo, "*")));
        // ---------------------------
        RowDefinition _rowDefinition5 = new RowDefinition();
        _rowDefinitionCollection1.Add(_rowDefinition5);
        _rowDefinition5.Height = ((GridLength)(_gridLengthConverter.ConvertFrom(null, EnglishCultureInfo, "*")));
        // ---------------------------
        RowDefinition _rowDefinition6 = new RowDefinition();
        _rowDefinitionCollection1.Add(_rowDefinition6);
        _rowDefinition6.Height = ((GridLength)(_gridLengthConverter.ConvertFrom(null, EnglishCultureInfo, "*")));
        // ---------------------------
        ColumnDefinitionCollection _columnDefinitionCollection7 = _grid0.ColumnDefinitions;
        // ---------------------------
        ColumnDefinition _columnDefinition8 = new ColumnDefinition();
        _columnDefinitionCollection7.Add(_columnDefinition8);
        _columnDefinition8.Width = ((GridLength)(_gridLengthConverter.ConvertFrom(null, EnglishCultureInfo, "*")));
        // ---------------------------
        ColumnDefinition _columnDefinition9 = new ColumnDefinition();
        _columnDefinitionCollection7.Add(_columnDefinition9);
        _columnDefinition9.Width = ((GridLength)(_gridLengthConverter.ConvertFrom(null, EnglishCultureInfo, "*")));
        // ---------------------------
        ColumnDefinition _columnDefinition10 = new ColumnDefinition();
        _columnDefinitionCollection7.Add(_columnDefinition10);
        _columnDefinition10.Width = ((GridLength)(_gridLengthConverter.ConvertFrom(null, EnglishCultureInfo, "*")));
        // ---------------------------
        ColumnDefinition _columnDefinition11 = new ColumnDefinition();
        _columnDefinitionCollection7.Add(_columnDefinition11);
        _columnDefinition11.Width = ((GridLength)(_gridLengthConverter.ConvertFrom(null, EnglishCultureInfo, "*")));
        // ---------------------------
        UIElementCollection _uIElementCollection12 = _grid0.Children;
        // ---------------------------
        lblNumbers = new Label();
        _uIElementCollection12.Add(lblNumbers);
        Grid.SetColumnSpan(lblNumbers, 4);
        lblNumbers.Content = "";
        ThicknessConverter _thicknessConverter = new ThicknessConverter();
        lblNumbers.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        lblNumbers.Foreground = Brushes.Red;
        lblNumbers.HorizontalContentAlignment = HorizontalAlignment.Center;
        lblNumbers.VerticalContentAlignment = VerticalAlignment.Center;
        lblNumbers.FontSize = 20;
        FontWeightConverter _fontWeightConverter = new FontWeightConverter();
        lblNumbers.FontWeight = ((FontWeight)(_fontWeightConverter.ConvertFrom(null, EnglishCultureInfo, "DemiBold")));
        lblNumbers.BorderBrush = Brushes.Black;
        lblNumbers.BorderThickness = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "1")));
        // ---------------------------
        Button _button13 = new Button();
        _uIElementCollection12.Add(_button13);
        Grid.SetRow(_button13, 1);
        // ---------------------------
        Binding _binding14 = new Binding("");
        _binding14.Source = ClickMeCommand;
        _button13.SetBinding(System.Windows.Controls.Button.CommandProperty, _binding14);
        _button13.CommandParameter = "7";
        BrushConverter _brushConverter = new BrushConverter();
        _button13.Background = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#a83271")));
        _button13.Foreground = Brushes.White;
        _button13.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button13.Content = "7";
        _button13.BorderBrush = Brushes.Black;
        _button13.BorderThickness = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "2")));
        // ---------------------------
        Button _button15 = new Button();
        _uIElementCollection12.Add(_button15);
        Grid.SetRow(_button15, 2);
        // ---------------------------
        Binding _binding16 = new Binding("");
        _binding16.Source = ClickMeCommand;
        _button15.SetBinding(System.Windows.Controls.Button.CommandProperty, _binding16);
        _button15.CommandParameter = "4";
        _button15.Background = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#bd3d2a")));
        _button15.Foreground = Brushes.White;
        _button15.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button15.Content = "4";
        _button15.BorderBrush = Brushes.Black;
        _button15.BorderThickness = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "2")));
        // ---------------------------
        Button _button17 = new Button();
        _uIElementCollection12.Add(_button17);
        Grid.SetRow(_button17, 3);
        // ---------------------------
        Binding _binding18 = new Binding("");
        _binding18.Source = ClickMeCommand;
        _button17.SetBinding(System.Windows.Controls.Button.CommandProperty, _binding18);
        _button17.CommandParameter = "1";
        _button17.Background = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#53b01e")));
        _button17.Foreground = Brushes.White;
        _button17.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button17.Content = "1";
        _button17.BorderBrush = Brushes.Black;
        _button17.BorderThickness = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "2")));
        // ---------------------------
        Button _button19 = new Button();
        _uIElementCollection12.Add(_button19);
        Grid.SetRow(_button19, 4);
        Grid.SetColumn(_button19, 3);
        // ---------------------------
        Binding _binding20 = new Binding("");
        _binding20.Source = ClickEnterCommand;
        _button19.SetBinding(System.Windows.Controls.Button.CommandProperty, _binding20);
        _button19.Background = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#1e4ab0")));
        _button19.Foreground = Brushes.Black;
        _button19.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button19.Content = "ENTER";
        _button19.BorderBrush = Brushes.Black;
        _button19.BorderThickness = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "2")));
        // ---------------------------
        Button _button21 = new Button();
        _uIElementCollection12.Add(_button21);
        Grid.SetRow(_button21, 1);
        Grid.SetColumn(_button21, 3);
        // ---------------------------
        Binding _binding22 = new Binding("");
        _binding22.Source = ClickDelCommand;
        _button21.SetBinding(System.Windows.Controls.Button.CommandProperty, _binding22);
        _button21.Background = Brushes.Red;
        _button21.Foreground = Brushes.White;
        _button21.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button21.Content = "DEL";
        _button21.BorderBrush = Brushes.Black;
        _button21.BorderThickness = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "2")));
        // ---------------------------
        Button _button23 = new Button();
        _uIElementCollection12.Add(_button23);
        Grid.SetRow(_button23, 2);
        Grid.SetColumn(_button23, 3);
        // ---------------------------
        Binding _binding24 = new Binding("");
        _binding24.Source = ClickPlusCommand;
        _button23.SetBinding(System.Windows.Controls.Button.CommandProperty, _binding24);
        _button23.Background = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#1e4ab0")));
        _button23.Foreground = Brushes.Black;
        _button23.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button23.Content = "+";
        _button23.BorderBrush = Brushes.White;
        _button23.BorderThickness = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "2")));
        // ---------------------------
        Button _button25 = new Button();
        _uIElementCollection12.Add(_button25);
        Grid.SetRow(_button25, 3);
        Grid.SetColumn(_button25, 3);
        // ---------------------------
        Binding _binding26 = new Binding("");
        _binding26.Source = ClickMinusCommand;
        _button25.SetBinding(System.Windows.Controls.Button.CommandProperty, _binding26);
        _button25.Background = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#1e4ab0")));
        _button25.Foreground = Brushes.Black;
        _button25.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button25.Content = "-";
        _button25.BorderBrush = Brushes.White;
        _button25.BorderThickness = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "2")));
        // ---------------------------
        Button _button27 = new Button();
        _uIElementCollection12.Add(_button27);
        Grid.SetColumn(_button27, 1);
        Grid.SetRow(_button27, 1);
        // ---------------------------
        Binding _binding28 = new Binding("");
        _binding28.Source = ClickMeCommand;
        _button27.SetBinding(System.Windows.Controls.Button.CommandProperty, _binding28);
        _button27.CommandParameter = "8";
        _button27.Background = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#a83271")));
        _button27.Foreground = Brushes.White;
        _button27.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button27.Content = "8";
        _button27.BorderBrush = Brushes.Black;
        _button27.BorderThickness = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "2")));
        // ---------------------------
        Button _button29 = new Button();
        _uIElementCollection12.Add(_button29);
        Grid.SetColumn(_button29, 1);
        Grid.SetRow(_button29, 2);
        // ---------------------------
        Binding _binding30 = new Binding("");
        _binding30.Source = ClickMeCommand;
        _button29.SetBinding(System.Windows.Controls.Button.CommandProperty, _binding30);
        _button29.CommandParameter = "5";
        _button29.Background = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#bd3d2a")));
        _button29.Foreground = Brushes.White;
        _button29.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button29.Content = "5";
        _button29.BorderBrush = Brushes.Black;
        _button29.BorderThickness = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "2")));
        // ---------------------------
        Button _button31 = new Button();
        _uIElementCollection12.Add(_button31);
        Grid.SetColumn(_button31, 1);
        Grid.SetRow(_button31, 3);
        // ---------------------------
        Binding _binding32 = new Binding("");
        _binding32.Source = ClickMeCommand;
        _button31.SetBinding(System.Windows.Controls.Button.CommandProperty, _binding32);
        _button31.CommandParameter = "2";
        _button31.Background = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#53b01e")));
        _button31.Foreground = Brushes.White;
        _button31.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button31.Content = "2";
        _button31.BorderBrush = Brushes.Black;
        _button31.BorderThickness = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "2")));
        // ---------------------------
        Button _button33 = new Button();
        _uIElementCollection12.Add(_button33);
        Grid.SetColumn(_button33, 0);
        Grid.SetRow(_button33, 4);
        Grid.SetColumnSpan(_button33, 2);
        // ---------------------------
        Binding _binding34 = new Binding("");
        _binding34.Source = ClickMeCommand;
        _button33.SetBinding(System.Windows.Controls.Button.CommandProperty, _binding34);
        _button33.CommandParameter = "0";
        _button33.Background = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#1e4ab0")));
        _button33.Foreground = Brushes.White;
        _button33.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button33.Content = "0";
        _button33.BorderBrush = Brushes.Black;
        _button33.BorderThickness = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "2")));
        // ---------------------------
        Button _button35 = new Button();
        _uIElementCollection12.Add(_button35);
        Grid.SetColumn(_button35, 2);
        Grid.SetRow(_button35, 1);
        // ---------------------------
        Binding _binding36 = new Binding("");
        _binding36.Source = ClickMeCommand;
        _button35.SetBinding(System.Windows.Controls.Button.CommandProperty, _binding36);
        _button35.CommandParameter = "9";
        _button35.Background = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#a83271")));
        _button35.Foreground = Brushes.White;
        _button35.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button35.Content = "9";
        _button35.BorderBrush = Brushes.Black;
        _button35.BorderThickness = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "2")));
        // ---------------------------
        Button _button37 = new Button();
        _uIElementCollection12.Add(_button37);
        Grid.SetColumn(_button37, 2);
        Grid.SetRow(_button37, 2);
        // ---------------------------
        Binding _binding38 = new Binding("");
        _binding38.Source = ClickMeCommand;
        _button37.SetBinding(System.Windows.Controls.Button.CommandProperty, _binding38);
        _button37.CommandParameter = "6";
        _button37.Background = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#bd3d2a")));
        _button37.Foreground = Brushes.White;
        _button37.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button37.Content = "6";
        _button37.BorderBrush = Brushes.Black;
        _button37.BorderThickness = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "2")));
        // ---------------------------
        Button _button39 = new Button();
        _uIElementCollection12.Add(_button39);
        Grid.SetColumn(_button39, 2);
        Grid.SetRow(_button39, 3);
        // ---------------------------
        Binding _binding40 = new Binding("");
        _binding40.Source = ClickMeCommand;
        _button39.SetBinding(System.Windows.Controls.Button.CommandProperty, _binding40);
        _button39.CommandParameter = "3";
        _button39.Background = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#53b01e")));
        _button39.Foreground = Brushes.White;
        _button39.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button39.Content = "3";
        _button39.BorderBrush = Brushes.Black;
        _button39.BorderThickness = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "2")));
        // ---------------------------
        Button _button41 = new Button();
        _uIElementCollection12.Add(_button41);
        Grid.SetColumn(_button41, 2);
        Grid.SetRow(_button41, 4);
        // ---------------------------
        Binding _binding42 = new Binding("");
        _binding42.Source = ClickMeCommand;
        _button41.SetBinding(System.Windows.Controls.Button.CommandProperty, _binding42);
        _button41.CommandParameter = ".";
        _button41.Background = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#1e4ab0")));
        _button41.Foreground = Brushes.White;
        _button41.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button41.Content = ".";
        _button41.BorderBrush = Brushes.Black;
        _button41.BorderThickness = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "2")));
    }

    void onMinus(object obj)
    {
        if (!string.IsNullOrEmpty(lblNumbers.Content))
        {
            Num1 = Convert.ToUInt32(lblNumbers.Content);
        }

        if (Num1 == 0)
        {
            Num1 = Num2;
        }

        opreation = Opreation.Minus;
        Num2 = 0;
        lblNumbers.Content = String.Empty;
    }

    void onPlus(object obj)
    {
        if (!string.IsNullOrEmpty(lblNumbers.Content))
        {
            Num1 = Convert.ToUInt32(lblNumbers.Content);
        }

        if (Num1 == 0)
        {
            Num1 = Num2;
        }

        opreation = Opreation.Plus;
        Num2 = 0;
        lblNumbers.Content = String.Empty;
    }

    void onDelete(object obj)
    {
        if (!String.IsNullOrEmpty(lblNumbers.Content))
        {
            lblNumbers.Content = lblNumbers.Content.Remove(lblNumbers.Content.Length);
        }
    }

    void onEnter(object obj)
    {
        switch (opreation )
        {
            case Opreation.Plus:
                Num1 += Num2;
                break;
            case Opreation.Minus:
                if (Num1 >= Num2)
                {
                    Num1 -= Num2;
                }
                else
                {
                    Num1 = 0;
                    Num2 = 0;
                }
                break;
            default:
                Num2 = 0;
                break;
        }
        lblNumbers.Content = Num1.ToString();
    }

    void onClick(object obj)
    {
        lblNumbers.Content += obj.ToString();
    }

}
