using Microsoft.EntityFrameworkCore;
using NorthwindCore.DataAccess;
using NorthwindCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace NorthwindCore.Tests
{
    public class RepositoryTests : IDisposable
    {
        private Repository repository;

        public RepositoryTests()
        {
            DbContextOptions<NorthwindContext> options;
            var builder = new DbContextOptionsBuilder<NorthwindContext>();
            builder.UseInMemoryDatabase("Northwind");
            options = builder.Options;
            NorthwindContext context = new NorthwindContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            repository = new Repository(context);
        }

        public void Dispose()
        {
            repository.Dispose();
        }

        [Fact]
        public void GetEmployeesTest()
        {
            repository.AddEmployee(new Employee() { FirstName = "Olga" });
            repository.AddEmployee(new Employee() { FirstName = "Peter" });

            Assert.True(repository.GetEmployees().Count == 2);
        }

        [Fact]
        public void AddEmployeeTest()
        {
            Employee employee = new Employee
            {
                FirstName = "Olga",
                LastName = "Petersen"
            };

            repository.AddEmployee(employee);
            Assert.True(repository.GetEmployees().Count == 1);
        }

        [Fact]
        public void AddEmployment()
        {
            Employee employee = new Employee
            {
                FirstName = "Olga",
                LastName = "Petersen"
            };
            repository.AddEmployee(employee);
            List<Employee> employees = repository.GetEmployees();
            employee = employees.FirstOrDefault(e => e.FirstName == "Olga");

            Employment employment = new Employment
            {
                EmployeeId = employee.EmployeeId,
                HireDate = DateTime.Now
            };

            repository.AddEmployment(employment, employee);
            Assert.True(employee.Employments.Count == 1);
        }

        [Fact]
        public void UpdateEmployeeTest()
        {
            Employee employee = new Employee
            {
                FirstName = "Olga",
                LastName = "Petersen"
            };
            repository.AddEmployee(employee);
            employee = repository.GetEmployees().FirstOrDefault(e => e.FirstName == "Olga");

            employee.LastName = "Henriksen";
            repository.UpdateEmployee(employee);

            Assert.True(repository.GetEmployees().First().LastName == "Henriksen");
        }

        [Fact]
        public void UpdateEmploymentTest()
        {
            Employee employee = new Employee
            {
                FirstName = "Olga",
                LastName = "Petersen"
            };
            repository.AddEmployee(employee);
            List<Employee> employees = repository.GetEmployees();
            employee = employees.FirstOrDefault(e => e.FirstName == "Olga");

            Employment employment = new Employment
            {
                EmployeeId = employee.EmployeeId,
                HireDate = DateTime.Now,
                Title = "Test"
            };
            repository.AddEmployment(employment, employee);
            employment = employee.Employments.FirstOrDefault(e => e.Title == "Test");

            employment.Title = "NotTest";
            repository.UpdateEmployment(employment);

            Assert.True(employee.Employments.First().Title == "NotTest");
        }
    }
}
