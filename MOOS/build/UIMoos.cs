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
    public System.Windows.Controls.Label lblNumbers;

    public UIMoos()
    {
        ClickMeCommand = new ICommand(onClick);
        ClickEnterCommand = new ICommand(onEnter);

        this.Title = "MOOS GUI";
        this.Width = 300;
        this.Height = 350;
        BrushConverter _brushConverter = new BrushConverter();
        this.Background = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#212121")));
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
        lblNumbers.Foreground = Brushes.White;
        lblNumbers.HorizontalContentAlignment = HorizontalAlignment.Center;
        lblNumbers.VerticalContentAlignment = VerticalAlignment.Center;
        lblNumbers.FontSize = 20;
        FontWeightConverter _fontWeightConverter = new FontWeightConverter();
        lblNumbers.FontWeight = ((FontWeight)(_fontWeightConverter.ConvertFrom(null, EnglishCultureInfo, "DemiBold")));
        // ---------------------------
        Button _button13 = new Button();
        _uIElementCollection12.Add(_button13);
        Grid.SetRow(_button13, 1);
        // ---------------------------
        Binding _binding14 = new Binding("");
        _binding14.Source = ClickMeCommand;
        _button13.SetBinding(System.Windows.Controls.Button.CommandProperty, _binding14);
        _button13.CommandParameter = "7";
        _button13.Background = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#a83271")));
        _button13.Foreground = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#ffffff")));
        ThicknessConverter _thicknessConverter = new ThicknessConverter();
        _button13.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button13.Content = "7";
        _button13.BorderBrush = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#ffffff")));
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
        _button15.Foreground = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#ffffff")));
        _button15.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button15.Content = "4";
        _button15.BorderBrush = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#ffffff")));
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
        _button17.Foreground = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#ffffff")));
        _button17.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button17.Content = "1";
        _button17.BorderBrush = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#ffffff")));
        _button17.BorderThickness = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "2")));
        // ---------------------------
        Button _button19 = new Button();
        _uIElementCollection12.Add(_button19);
        Grid.SetRow(_button19, 1);
        Grid.SetColumn(_button19, 3);
        Grid.SetRowSpan(_button19, 4);
        // ---------------------------
        Binding _binding20 = new Binding("");
        _binding20.Source = ClickEnterCommand;
        _button19.SetBinding(System.Windows.Controls.Button.CommandProperty, _binding20);
        _button19.Background = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#1e4ab0")));
        _button19.Foreground = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#ffffff")));
        _button19.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button19.Content = "ENTER";
        _button19.BorderBrush = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#ffffff")));
        _button19.BorderThickness = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "2")));
        // ---------------------------
        Button _button21 = new Button();
        _uIElementCollection12.Add(_button21);
        Grid.SetColumn(_button21, 1);
        Grid.SetRow(_button21, 1);
        // ---------------------------
        Binding _binding22 = new Binding("");
        _binding22.Source = ClickMeCommand;
        _button21.SetBinding(System.Windows.Controls.Button.CommandProperty, _binding22);
        _button21.CommandParameter = "8";
        _button21.Background = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#a83271")));
        _button21.Foreground = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#ffffff")));
        _button21.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button21.Content = "8";
        _button21.BorderBrush = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#ffffff")));
        _button21.BorderThickness = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "2")));
        // ---------------------------
        Button _button23 = new Button();
        _uIElementCollection12.Add(_button23);
        Grid.SetColumn(_button23, 1);
        Grid.SetRow(_button23, 2);
        // ---------------------------
        Binding _binding24 = new Binding("");
        _binding24.Source = ClickMeCommand;
        _button23.SetBinding(System.Windows.Controls.Button.CommandProperty, _binding24);
        _button23.CommandParameter = "5";
        _button23.Background = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#bd3d2a")));
        _button23.Foreground = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#ffffff")));
        _button23.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button23.Content = "5";
        _button23.BorderBrush = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#ffffff")));
        _button23.BorderThickness = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "2")));
        // ---------------------------
        Button _button25 = new Button();
        _uIElementCollection12.Add(_button25);
        Grid.SetColumn(_button25, 1);
        Grid.SetRow(_button25, 3);
        // ---------------------------
        Binding _binding26 = new Binding("");
        _binding26.Source = ClickMeCommand;
        _button25.SetBinding(System.Windows.Controls.Button.CommandProperty, _binding26);
        _button25.CommandParameter = "2";
        _button25.Background = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#53b01e")));
        _button25.Foreground = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#ffffff")));
        _button25.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button25.Content = "2";
        _button25.BorderBrush = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#ffffff")));
        _button25.BorderThickness = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "2")));
        // ---------------------------
        Button _button27 = new Button();
        _uIElementCollection12.Add(_button27);
        Grid.SetColumn(_button27, 0);
        Grid.SetRow(_button27, 4);
        Grid.SetColumnSpan(_button27, 2);
        // ---------------------------
        Binding _binding28 = new Binding("");
        _binding28.Source = ClickMeCommand;
        _button27.SetBinding(System.Windows.Controls.Button.CommandProperty, _binding28);
        _button27.CommandParameter = "0";
        _button27.Background = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#1e4ab0")));
        _button27.Foreground = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#ffffff")));
        _button27.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button27.Content = "0";
        _button27.BorderBrush = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#ffffff")));
        _button27.BorderThickness = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "2")));
        // ---------------------------
        Button _button29 = new Button();
        _uIElementCollection12.Add(_button29);
        Grid.SetColumn(_button29, 2);
        Grid.SetRow(_button29, 1);
        // ---------------------------
        Binding _binding30 = new Binding("");
        _binding30.Source = ClickMeCommand;
        _button29.SetBinding(System.Windows.Controls.Button.CommandProperty, _binding30);
        _button29.CommandParameter = "9";
        _button29.Background = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#a83271")));
        _button29.Foreground = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#ffffff")));
        _button29.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button29.Content = "9";
        _button29.BorderBrush = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#ffffff")));
        _button29.BorderThickness = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "2")));
        // ---------------------------
        Button _button31 = new Button();
        _uIElementCollection12.Add(_button31);
        Grid.SetColumn(_button31, 2);
        Grid.SetRow(_button31, 2);
        // ---------------------------
        Binding _binding32 = new Binding("");
        _binding32.Source = ClickMeCommand;
        _button31.SetBinding(System.Windows.Controls.Button.CommandProperty, _binding32);
        _button31.CommandParameter = "6";
        _button31.Background = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#bd3d2a")));
        _button31.Foreground = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#ffffff")));
        _button31.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button31.Content = "6";
        _button31.BorderBrush = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#ffffff")));
        _button31.BorderThickness = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "2")));
        // ---------------------------
        Button _button33 = new Button();
        _uIElementCollection12.Add(_button33);
        Grid.SetColumn(_button33, 2);
        Grid.SetRow(_button33, 3);
        // ---------------------------
        Binding _binding34 = new Binding("");
        _binding34.Source = ClickMeCommand;
        _button33.SetBinding(System.Windows.Controls.Button.CommandProperty, _binding34);
        _button33.CommandParameter = "3";
        _button33.Background = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#53b01e")));
        _button33.Foreground = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#ffffff")));
        _button33.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button33.Content = "3";
        _button33.BorderBrush = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#ffffff")));
        _button33.BorderThickness = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "2")));
        // ---------------------------
        Button _button35 = new Button();
        _uIElementCollection12.Add(_button35);
        Grid.SetColumn(_button35, 2);
        Grid.SetRow(_button35, 4);
        // ---------------------------
        Binding _binding36 = new Binding("");
        _binding36.Source = ClickMeCommand;
        _button35.SetBinding(System.Windows.Controls.Button.CommandProperty, _binding36);
        _button35.CommandParameter = ".";
        _button35.Background = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#1e4ab0")));
        _button35.Foreground = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#ffffff")));
        _button35.Margin = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "5")));
        _button35.Content = ".";
        _button35.BorderBrush = ((Brush)(_brushConverter.ConvertFrom(null, EnglishCultureInfo, "#ffffff")));
        _button35.BorderThickness = ((Thickness)(_thicknessConverter.ConvertFrom(null, EnglishCultureInfo, "2")));
    }

    void onEnter(object obj)
    {
        MessageBox.Show("Clicked on Enter!","UIXaml Moos");
    }

    void onClick(object obj)
    {
        lblNumbers.Content += obj.ToString();
    }

}
