using NorthwindCore.Entities;
using NorthwindCore.Gui.Desktop.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NorthwindCore.Gui.Desktop
{
    /// <summary>
    /// Interaction logic for AddEmployment.xaml
    /// </summary>
    public partial class AddEmployment : Window
    {
        public readonly AddEmploymentViewModel addEmploymentViewModel;

        public AddEmployment(Employee selectedEmployee)
        {
            InitializeComponent();
            addEmploymentViewModel = new AddEmploymentViewModel(selectedEmployee);
            DataContext = addEmploymentViewModel;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxTitle.Text))
            {
                MessageBox.Show("Udfyld venligst alle felterne", "Tilføj ansættelsesforhold", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DialogResult = true;
        }
    }
}
