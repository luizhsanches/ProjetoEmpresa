using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoEmpresa.Models.Repositories
{
    public interface IDatabase
    {
        ObservableCollection<Employee> GetEmployees();
        void InsertEmployee(Employee newEmployee);
        void UpdateEmployee(Employee employeeToUpdate);
        void DeleteEmployee(int employeeId);

    }
}
