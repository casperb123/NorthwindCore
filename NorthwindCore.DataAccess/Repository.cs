using Microsoft.EntityFrameworkCore;
using NorthwindCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime;

namespace NorthwindCore.DataAccess
{
    public class Repository
    {
        NorthwindContext context = new NorthwindContext();

        public List<Employee> GetEmployees()
        {
            return context.Employees.Include(e => e.Employments).ToList();
        }

        public async IAsyncEnumerable<Employee> GetEmployeesAsync()
        {
            await foreach (Employee employee in context.Employees.Include(e => e.Employments).AsAsyncEnumerable())
            {
                yield return employee;
            }
        }

        public void Update(Employee employee)
        {
            context.Employees.Update(employee);
            context.SaveChanges();
        }

        public void Update(Employment employment)
        {
            context.Employments.Update(employment);
            context.SaveChanges();
        }

        public void Insert(Employment employment, Employee employee)
        {
            context.Employees.Update(employee);
            context.Employments.Add(employment);
            context.SaveChanges();
        }
    }
}
