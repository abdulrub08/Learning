using Kudvenkat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kudvenkat.Repositry
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;

        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>()
        {
            new Employee() { ID = 1, Name = "Mary", Department = Dept.HR, Email = "mary@pragimtech.com" },
            new Employee() { ID = 2, Name = "John", Department = Dept.IT, Email = "john@pragimtech.com" },
            new Employee() { ID = 3, Name = "Sam", Department = Dept.Payroll, Email = "sam@pragimtech.com" },
        };
        }

        public Employee GetEmployee(int Id)
        {
            return this._employeeList.FirstOrDefault(e => e.ID == Id);
        }
        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeList;
        }

        public Employee Add(Employee employee)
        {
            employee.ID = _employeeList.Max(e => e.ID) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Update(Employee employeeChanges)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.ID == employeeChanges.ID);
            if (employee != null)
            {
                employee.Name = employeeChanges.Name;
                employee.Email = employeeChanges.Email;
                employee.Department = employeeChanges.Department;
            }
            return employee;
        }
        public Employee Delete(int Id)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.ID == Id);
            if (employee != null)
            {
                _employeeList.Remove(employee);
            }
            return employee;
        }
    }
}
