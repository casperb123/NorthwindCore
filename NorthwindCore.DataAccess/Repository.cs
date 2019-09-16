using Microsoft.EntityFrameworkCore;
using NorthwindCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime;

namespace NorthwindCore.DataAccess
{
    public class Repository : IDisposable
    {
        private readonly NorthwindContext context;

        public Repository(NorthwindContext context = null)
        {
            if (context is null)
            {
                this.context = new NorthwindContext();
            }
            else
            {
                this.context = context;
            }
        }

        /// <summary>
        /// Returns a list with all employees
        /// </summary>
        /// <returns>List of employees</returns>
        public List<Employee> GetEmployees()
        {
            return context.Employees.Include(e => e.Employments).ToList();
        }

        /// <summary>
        /// Returns a list with all employees asynchronously
        /// </summary>
        /// <returns>List of employees</returns>
        public async IAsyncEnumerable<Employee> GetEmployeesAsync()
        {
            await foreach (Employee employee in context.Employees.Include(e => e.Employments).AsAsyncEnumerable())
            {
                yield return employee;
            }
        }

        /// <summary>
        /// Adds an employment to the database
        /// </summary>
        /// <param name="employment">The employment to add</param>
        /// <param name="employee">The employee to add the employmet to</param>
        public void AddEmployment(Employment employment, Employee employee)
        {
            context.Employees.Update(employee);
            context.Employments.Add(employment);
            context.SaveChanges();
        }

        /// <summary>
        /// Adds a employee to the database
        /// </summary>
        /// <param name="employee">The employee to add</param>
        public void AddEmployee(Employee employee)
        {
            context.Employees.Add(employee);
            context.SaveChanges();
        }

        /// <summary>
        /// Updates a employee on the database
        /// </summary>
        /// <param name="employee">The employee to update</param>
        public void UpdateEmployee(Employee employee)
        {
            context.Employees.Update(employee);
            context.SaveChanges();
        }

        /// <summary>
        /// Updates an employment on the database
        /// </summary>
        /// <param name="employment">The employment to update</param>
        public void UpdateEmployment(Employment employment)
        {
            context.Employments.Update(employment);
            context.SaveChanges();
        }

        /// <summary>
        /// Deletes an employment on the database
        /// </summary>
        /// <param name="employment">The employment to delete</param>
        public void DeleteEmployment(Employment employment)
        {
            Employment employmentOnDb = context.Employments.FirstOrDefault(e => e.Id == employment.Id);
            Employee employee = context.Employees.FirstOrDefault(e => e.EmployeeId == employmentOnDb.EmployeeId);
            employee.Employments.Remove(employmentOnDb);
            context.Employments.Remove(employmentOnDb);
            context.SaveChanges();
        }

        /// <summary>
        /// Disposes the repository
        /// </summary>
        public void Dispose()
        {
            context.Dispose();
        }
    }
}
