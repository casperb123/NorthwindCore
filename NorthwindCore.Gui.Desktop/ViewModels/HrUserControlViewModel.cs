using NorthwindCore.DataAccess;
using NorthwindCore.Entities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using NorthwindCore.Services;

namespace NorthwindCore.Gui.Desktop.ViewModels
{
    public class HrUserControlViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Employee> employees;
        private Employee selectedEmployee;
        private Employment selectedEmployment;

        /// <summary>
        /// The list of employees
        /// </summary>
        public ObservableCollection<Employee> Employees
        {
            get { return employees; }
            set
            {
                if (value is null)
                {
                    throw new NullReferenceException("Listen med ansatte kan ikke være null");
                }

                employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }

        /// <summary>
        /// The selected employment
        /// </summary>
        public Employment SelectedEmployment
        {
            get { return selectedEmployment; }
            set
            {
                selectedEmployment = value;
                OnPropertyChanged("SelectedEmployment");
            }
        }

        /// <summary>
        /// The selected employee
        /// </summary>
        public Employee SelectedEmployee
        {
            get { return selectedEmployee; }
            set
            {
                if (value is null)
                {
                    throw new NullReferenceException("En ansat kan ikke være null");
                }

                selectedEmployee = value;
                OnPropertyChanged("SelectedEmployee");
            }
        }

        public HrUserControlViewModel()
        {
            employees = new ObservableCollection<Employee>();
            Task.Factory.StartNew(() => GetEmployees());
        }

        /// <summary>
        /// Gets a list of all employees and then adds them to the employee list
        /// </summary>
        private async void GetEmployees()
        {
            Repository repository = new Repository();
            await foreach (Employee employee in repository.GetEmployeesAsync())
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    Employees.Add(employee);
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string prop)
        {
            if (prop != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        /// <summary>
        /// Updates a employee on the database
        /// </summary>
        /// <param name="employee">The employee to update</param>
        public void UpdateEmployee(Employee employee)
        {
            Repository repository = new Repository();
            repository.UpdateEmployee(employee);
        }

        /// <summary>
        /// Updates an employment on the database
        /// </summary>
        /// <param name="employment">The employment to update</param>
        public void UpdateEmployment(Employment employment)
        {
            Repository repository = new Repository();
            repository.UpdateEmployment(employment);
        }

        /// <summary>
        /// Adds an employment to the database
        /// </summary>
        /// <param name="employment">The employment to add</param>
        /// <param name="employee">The employee to add the employment to</param>
        public void AddEmployment(Employment employment, Employee employee)
        {
            Repository repository = new Repository();
            repository.AddEmployment(employment, employee);
        }

        /// <summary>
        /// Deletes an employment on the database
        /// </summary>
        /// <param name="employment">The employment to delete</param>
        public void DeleteEmployment(Employment employment)
        {
            Repository repository = new Repository();
            repository.DeleteEmployment(employment);
            SelectedEmployee.Employments.Remove(employment);
        }

        /// <summary>
        /// Checks if the phone number is valid
        /// </summary>
        /// <param name="employee">The employee to check</param>
        /// <returns>A bool that indicates if the phone number is valid</returns>
        public bool IsPhoneNumberValid(Employee employee)
        {
            ValidationWebService validationWebService = new ValidationWebService();
            return validationWebService.ValidatePhoneNumber(employee);
        }

        /// <summary>
        /// Checks if the notes is valid
        /// </summary>
        /// <param name="employee">The employee to check</param>
        /// <returns>The notes without any profanity</returns>
        public string ValidateNotes(Employee employee)
        {
            ValidationWebService validationWebService = new ValidationWebService();
            return validationWebService.ValidateNotes(employee.Notes);
        }
    }
}
