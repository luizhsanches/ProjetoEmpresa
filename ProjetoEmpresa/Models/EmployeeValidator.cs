using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjetoEmpresa.Models
{
    public interface IEmployeeValidator
    {
        void Validate(Employee employee);
    }
    public class EmployeeValidator : IEmployeeValidator
    {
        public EmployeeValidator() { }
        public void Validate(Employee employee)
        {
            if (string.IsNullOrWhiteSpace(employee.EmployeeName) || employee.EmployeeName.Length < 3)
                throw new ArgumentException("O nome do funcionario deve ter mais que 3 caracteres.");

            if (!Enum.IsDefined(typeof(DepartmentEnum), employee.Department))
                throw new ArgumentException("O campo Departamento não deve ser vazio.");

            if (string.IsNullOrWhiteSpace(employee.Address) || employee.Address.Length < 3)
                throw new ArgumentException("O endereço do funcionario deve ter mais que 3 caracteres.");
        }
    }
}
