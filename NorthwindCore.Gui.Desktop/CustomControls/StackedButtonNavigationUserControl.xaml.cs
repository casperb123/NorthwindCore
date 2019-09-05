using NorthwindCore.Gui.Desktop.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NorthwindCore.Gui.Desktop.CustomControls
{
    /// <summary>
    /// Interaction logic for StackedButtonNavigationUserControl.xaml
    /// </summary>
    public partial class StackedButtonNavigationUserControl : UserControl
    {
        private readonly StackedButtonNavigationUserControlViewModel stackedButtonNavigationUserControlViewModel;

        public StackedButtonNavigationUserControl(MainWindow mainWindow)
        {
            InitializeComponent();
            stackedButtonNavigationUserControlViewModel = new StackedButtonNavigationUserControlViewModel(mainWindow);
        }

        private void ButtonHr_Checked(object sender, RoutedEventArgs e)
        {
            stackedButtonNavigationUserControlViewModel.MainWindow.DetailsUserControl.Content = new HrUserControl();
        }
    }
}
