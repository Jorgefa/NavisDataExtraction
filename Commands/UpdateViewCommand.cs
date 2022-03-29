using NavisDataExtraction.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NavisDataExtraction.Commands
{
    public class UpdateViewCommand : ICommand
    {
        private MainWindowViewModel viewModel;

        public UpdateViewCommand(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
           return true;
        }

        public void Execute(object parameter)
        {
            Console.WriteLine("test");
            if (parameter.ToString() == "Extraction")
            {
                viewModel.SelectedViewModel = new ExtractionViewModel();
            }
            else if (parameter.ToString() == "Editor")
            {
                viewModel.SelectedViewModel = new EditorViewModel();
            }
        }
    }
}
