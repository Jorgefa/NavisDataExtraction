using System.Windows;
using Autodesk.Navisworks.Api;
using PM.Navisworks.DataExtraction.ViewModels;

namespace PM.Navisworks.DataExtraction.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(Document activeDoc)
        {
            DataContext = new MainWindowViewModel(activeDoc);
            InitializeComponent();
        }
    }
}