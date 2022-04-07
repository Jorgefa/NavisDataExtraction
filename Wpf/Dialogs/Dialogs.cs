using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavisDataExtraction.Wpf.Dialogs
{
    public static class Dialogs
    {
        public static string ShowInputDialog(string title = "Title", string message = "Message")
        {
            var dialog = new InputDialog(title, message);
            if(dialog.ShowDialog() == true)
            {
                return dialog.Input.Text;
            }
            return "";
        }

    }
}
