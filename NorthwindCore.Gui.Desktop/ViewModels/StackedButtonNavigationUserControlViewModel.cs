using System;
using System.Collections.Generic;
using System.Text;

namespace NorthwindCore.Gui.Desktop.ViewModels
{
    public class StackedButtonNavigationUserControlViewModel
    {
        public MainWindow MainWindow { get; private set; }

        public StackedButtonNavigationUserControlViewModel(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
        }
    }
}
