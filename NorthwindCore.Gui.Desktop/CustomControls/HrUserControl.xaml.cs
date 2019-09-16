using NorthwindCore.Entities;
using NorthwindCore.Gui.Desktop.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
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

namespace NorthwindCore.Gui.Desktop.CustomControls
{
    /// <summary>
    /// Interaction logic for HrUserControl.xaml
    /// </summary>
    public partial class HrUserControl : UserControl
    {
        private readonly HrUserControlViewModel hrUserControlViewModel;

        public HrUserControl()
        {
            InitializeComponent();
            hrUserControlViewModel = new HrUserControlViewModel();
            DataContext = hrUserControlViewModel;
        }

        private void ButtonEmployeeUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!hrUserControlViewModel.IsPhoneNumberValid(hrUserControlViewModel.SelectedEmployee))
            {
                MessageBox.Show("Telefon nummeret er et forkert format", "Northwind", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            hrUserControlViewModel.SelectedEmployee.Notes = hrUserControlViewModel.ValidateNotes(hrUserControlViewModel.SelectedEmployee);
            textBoxNotes.Text = hrUserControlViewModel.SelectedEmployee.Notes;
            hrUserControlViewModel.UpdateEmployee(hrUserControlViewModel.SelectedEmployee);

            buttonEmployeeUpdate.Foreground = Brushes.Green;
            buttonEmployeeUpdate.Content = "Opdateret!";
            Task.Delay(2000).ContinueWith(_ =>
            {
                Dispatcher.Invoke(() =>
                {
                    buttonEmployeeUpdate.Foreground = Brushes.Black;
                    buttonEmployeeUpdate.Content = "Opdatér";
                });
            });
        }

        private void DataGridEmployee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridEmployees.SelectedItem != null)
            {
                hrUserControlViewModel.SelectedEmployee = (Employee)dataGridEmployees.SelectedItem;
                comboBoxReportsTo.ItemsSource = hrUserControlViewModel.Employees.Where(em => em.EmployeeId != hrUserControlViewModel.SelectedEmployee.EmployeeId);
                buttonEmploymentAdd.IsEnabled = true;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textBoxInitials.Text = hrUserControlViewModel.SelectedEmployee.Initials;
        }

        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string nameText = textBoxSearchName.Text.ToLower();
            string titleText = textBoxSearchTitle.Text.ToLower();
            string countryText = textBoxSearchCountry.Text.ToLower();
            string regionText = textBoxSearchRegion.Text.ToLower();

            if (string.IsNullOrWhiteSpace(nameText)
                && string.IsNullOrWhiteSpace(titleText)
                && string.IsNullOrWhiteSpace(countryText)
                && string.IsNullOrWhiteSpace(regionText))
            {
                dataGridEmployees.ItemsSource = hrUserControlViewModel.Employees;
                return;
            }

            var itemSourceList = new CollectionViewSource() { Source = hrUserControlViewModel.Employees };
            ICollectionView itemList = itemSourceList.View;
            List<Predicate<Employee>> predicates = new List<Predicate<Employee>>();

            if (!string.IsNullOrWhiteSpace(nameText))
            {
                predicates.Add(employee => employee.FirstName.ToLower().Contains(nameText)
                                           || employee.LastName.ToLower().Contains(nameText)
                                           || employee.Initials != null
                                           && employee.Initials.ToLower().Contains(nameText)
                                           || employee.FirstName.ToLower().Contains(nameText.Split(' ')[0])
                                           && employee.LastName.ToLower().Contains(nameText.Split(' ')[1]));
            }
            if (!string.IsNullOrWhiteSpace(titleText))
            {
                predicates.Add(employee => employee.Title != null
                                           && employee.Title.ToLower().Contains(titleText));
            }
            if (!string.IsNullOrWhiteSpace(countryText))
            {
                predicates.Add(employee => employee.Country != null
                                           && employee.Country.ToLower().Contains(countryText));
            }
            if (!string.IsNullOrWhiteSpace(regionText))
            {
                predicates.Add(employee => employee.Region != null
                                           && employee.Region.ToLower().Contains(regionText));
            }

            Expression<Func<Employee, bool>> employeeWhere = employee => predicates.All(pred => pred(employee));
            var filter = new Predicate<object>(employee => employeeWhere.Compile().Invoke((Employee)employee));
            itemList.Filter = filter;
            dataGridEmployees.ItemsSource = itemList;
        }

        private void ButtonEmploymentUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxEmploymentTitle.Text) || datePickerEmploymentHireDate.SelectedDate == null)
            {
                MessageBox.Show("Udfyld venligst alle felterne", "Northwind", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var validation = hrUserControlViewModel.SelectedEmployee.ValidateEmployment(hrUserControlViewModel.SelectedEmployment);
            if (!validation.isValid)
            {
                MessageBox.Show(validation.message, "Northwind", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            hrUserControlViewModel.UpdateEmployment(hrUserControlViewModel.SelectedEmployment);

            buttonEmploymentUpdate.Content = "Opdateret!";
            buttonEmploymentUpdate.Foreground = Brushes.Green;
            Task.Delay(3000).ContinueWith(_ =>
            {
                Dispatcher.Invoke(() =>
                {
                    buttonEmploymentUpdate.Content = "Opdatér";
                    buttonEmploymentUpdate.Foreground = Brushes.Black;
                });
            });
        }

        private void DataGridEmployments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            hrUserControlViewModel.SelectedEmployment = (Employment)dataGridEmployments.SelectedItem;
        }

        private void ButtonEmploymentAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEmployment addEmployment = new AddEmployment(hrUserControlViewModel.SelectedEmployee);

            if (addEmployment.ShowDialog() == true)
            {
                var validation = hrUserControlViewModel.SelectedEmployee.ValidateEmployment(addEmployment.addEmploymentViewModel.Employment);
                if (!validation.isValid)
                {
                    MessageBox.Show(validation.message, "Northwind", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                hrUserControlViewModel.SelectedEmployee.Employments.Add(addEmployment.addEmploymentViewModel.Employment);
                hrUserControlViewModel.AddEmployment(addEmployment.addEmploymentViewModel.Employment, hrUserControlViewModel.SelectedEmployee);
                dataGridEmployments.SelectedItem = addEmployment.addEmploymentViewModel.Employment;
                dataGridEmployments.Focus();
            }
        }

        private void ContextMenuEmployments_Opened(object sender, RoutedEventArgs e)
        {
            if (hrUserControlViewModel.SelectedEmployment is null)
            {
                menuItemEmploymentsDelete.IsEnabled = false;
            }
            else
            {
                menuItemEmploymentsDelete.IsEnabled = true;
            }
        }

        private void MenuItemEmploymentsDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Er du sikker på at du vil slette ansættelsesforholdet?", "Slet ansættelsesforhold", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                hrUserControlViewModel.DeleteEmployment(hrUserControlViewModel.SelectedEmployment);
            }
        }
    }
}
