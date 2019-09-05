using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace NorthwindCore.Entities
{
    public partial class Employee : INotifyPropertyChanged
    {
        private ICollection<Employment> employments;
        private string initials;

        public Employee()
        {
            EmployeeTerritories = new HashSet<EmployeeTerritory>();
            employments = new ObservableHashSet<Employment>();
            InverseReportsToNavigation = new HashSet<Employee>();
            Orders = new HashSet<Order>();
        }

        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public string FullNameWithInitials
        {
            get
            {
                return $"{FullName} ({Initials})";
            }
        }

        public int EmployeeId { get; set; }

        [Required]
        [StringLength(20)]
        public string LastName { get; set; }

        [Required]
        [StringLength(10)]
        public string FirstName { get; set; }

        [StringLength(30)]
        public string Title { get; set; }

        [StringLength(25)]
        public string TitleOfCourtesy { get; set; }

        public DateTime? BirthDate { get; set; }

        [StringLength(60)]
        public string Address { get; set; }

        [StringLength(15)]
        public string City { get; set; }

        [StringLength(15)]
        public string Region { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }

        [StringLength(15)]
        public string Country { get; set; }

        [StringLength(24)]
        public string HomePhone { get; set; }

        [StringLength(4)]
        public string Extension { get; set; }

        [Column(TypeName = "image")]
        public byte[] Photo { get; set; }

        [Column(TypeName = "ntext")]
        public string Notes { get; set; }

        public int? ReportsTo { get; set; }

        [StringLength(255)]
        public string PhotoPath { get; set; }

        public string Initials
        {
            get
            {
                if (FirstName.Length >= 2
                    && LastName.Length >= 2
                    && !string.IsNullOrWhiteSpace(FirstName)
                    && !string.IsNullOrWhiteSpace(LastName))
                {
                    string firstNameInitials = FirstName.Substring(0, 2);
                    string lastNameInitials = LastName.Substring(0, 2);

                    return $"{firstNameInitials}{lastNameInitials}".ToUpper();
                }

                return string.Empty;
            }
        }

        public (bool isValid, string message) ValidateEmployment(Employment employment)
        {
            if (employment is null)
            {
                return (false, "Ansættelsesforholdet kan ikke være null");
            }

            if (employment.LeaveDate != null && employment.HireDate > employment.LeaveDate)
            {
                return (false, "Ansættelsesdatoen kan ikke være højere end fyringsdatoen");
            }

            List<Employment> employments = new List<Employment>(Employments);

            if (employments.Count == 1 && employments[0] == employment)
            {
                return (true, string.Empty);
            }

            if (employments.Contains(employment))
            {
                employments.Remove(employment);
            }

            if (employments.SingleOrDefault(e => e.LeaveDate is null && employment.LeaveDate is null) != null)
            {
                return (false, "En ansat kan ikke have flere jobs på samme tid");
            }

            if (employments.SingleOrDefault(e => e.LeaveDate is null && employment.LeaveDate is null || employment.HireDate < e.LeaveDate || e.HireDate > employment.LeaveDate) != null)
            {
                return (false, "En ansats ansættelsesforhold må ikke overlappe hindanden");
            }

            return (true, string.Empty);
        }

        public virtual Employee ReportsToNavigation { get; set; }
        public virtual ICollection<EmployeeTerritory> EmployeeTerritories { get; set; }
        public virtual ICollection<Employment> Employments
        {
            get { return employments; }
            set
            {
                if (value is null)
                {
                    throw new NullReferenceException("En ansats ansættelsesforhold kan ikke være null");
                }

                employments = value;
                OnPropertyChanged(nameof(Employments));
            }
        }
        public virtual ICollection<Employee> InverseReportsToNavigation { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string prop)
        {
            if (prop != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
