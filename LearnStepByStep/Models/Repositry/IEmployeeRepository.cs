using System;
using System.Collections.Generic;
using LearnStepByStep.Models;
using LearnStepByStep.Models.DOM;
using System.Linq;
using System.Threading.Tasks;

namespace LearnStepByStep.Models.Repositry
{
    public interface IEmployeeRepository
    {
        Employee GetEmployee(int Id);
        IEnumerable<Employee> GetAllEmployee();
        Employee Add(Employee employee);
        Employee Update(Employee employeeChanges);
        Employee Delete(int Id);
    }
}
