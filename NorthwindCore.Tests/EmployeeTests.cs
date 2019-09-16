using NorthwindCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NorthwindCore.Tests
{
    public class EmployeeTests
    {
        [Theory]
        [InlineData("Olga", "Petersen")]
        [InlineData("Per", "Jespersen")]
        [InlineData("Jørgen", "Hansen")]
        public void InitialsTest(string firstName, string lastName)
        {
            Employee employee = new Employee() { FirstName = firstName, LastName = lastName };
            string firstNameInitials = firstName.Substring(0, 2);
            string lastNameInitials = lastName.Substring(0, 2);
            string initials = $"{firstNameInitials}{lastNameInitials}".ToUpper();

            Assert.True(employee.Initials == initials);
        }

        [Theory]
        [InlineData("Olga", "Petersen")]
        [InlineData("Per", "Jespersen")]
        [InlineData("Jørgen", "Hansen")]
        public void ToStringTest(string firstName, string lastName)
        {
            Employee employee = new Employee() { FirstName = firstName, LastName = lastName };

            Assert.True(employee.ToString() == $"{firstName} {lastName}");
        }
    }
}
