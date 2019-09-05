using NorthwindCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NorthwindCore.Gui.Desktop.ViewModels
{
    public class AddEmploymentViewModel : INotifyPropertyChanged
    {
        private Employment employment;

        public Employment Employment
        {
            get { return employment; }
            set
            {
                if (value != null)
                {
                    employment = value;
                    OnPropertyChanged("Employment");
                }
            }
        }

        public AddEmploymentViewModel(Employee selectedEmployee)
        {
            employment = new Employment()
            {
                EmployeeId = selectedEmployee.EmployeeId,
                HireDate = DateTime.Now,
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string prop)
        {
            if (prop != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
