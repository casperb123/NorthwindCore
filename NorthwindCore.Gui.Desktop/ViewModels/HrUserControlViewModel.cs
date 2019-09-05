using NorthwindCore.DataAccess;
using NorthwindCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NorthwindCore.Gui.Desktop.ViewModels
{
    public class HrUserControlViewModel : INotifyPropertyChanged
    {
        private List<Employee> employees;
        private Employee selectedEmployee;
        private Employment selectedEmployment;

        public List<Employee> Employees
        {
            get { return employees; }
            set
            {
                if (value is null)
                {
                    throw new NullReferenceException("Listen med ansatte kan ikke være null");
                }

                employees = value;
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
            Repository repository = new Repository();
            employees = repository.GetEmployees();
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
    }
}
