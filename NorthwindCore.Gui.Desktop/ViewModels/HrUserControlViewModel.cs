using NorthwindCore.DataAccess;
using NorthwindCore.Entities;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace NorthwindCore.Gui.Desktop.ViewModels
{
    public class HrUserControlViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Employee> employees;
        private Employee selectedEmployee;
        private Employment selectedEmployment;

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

        public Employment SelectedEmployment
        {
            get { return selectedEmployment; }
            set
            {
                selectedEmployment = value;
                OnPropertyChanged("SelectedEmployment");
            }
        }

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
            //Repository repository = new Repository();
            //employees = repository.GetEmployees();
            Task.Factory.StartNew(() => GetEmployees());
        }

        private async void GetEmployees()
        {
            Employees = new ObservableCollection<Employee>();
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

        public void Update(Employee employee)
        {
            Repository repository = new Repository();
            repository.Update(employee);
        }

        public void Update(Employment employment)
        {
            Repository repository = new Repository();
            repository.Update(employment);
        }

        public void Insert(Employment employment, Employee employee)
        {
            Repository repository = new Repository();
            repository.Insert(employment, employee);
        }

        public void Delete(Employment employment)
        {
            Repository repository = new Repository();
            repository.Delete(employment);
            SelectedEmployee.Employments.Remove(employment);
        }
    }
}
