﻿using NavisDataExtraction.DataExport;
using NavisDataExtraction.Wpf.ViewModels;
using System;
using System.Collections.Generic;
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

namespace NavisDataExtraction.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Constructor
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Extraction_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new ExtractionViewModel();
        }

        private void Config_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new ConfigViewModel();
        }
    }
}
