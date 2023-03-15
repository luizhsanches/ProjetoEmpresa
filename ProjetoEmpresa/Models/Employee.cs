using ProjetoEmpresa.ViewModels;
using ProjetoEmpresa.ViewModels.VMUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProjetoEmpresa.Models
{
    public class Employee : BaseNotifier
    {
        private int _id;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name;
        public string EmployeeName 
        {
            get { return _name; }
            set {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(EmployeeName));
                }
            }
        }

        private DepartmentEnum _department;
        public DepartmentEnum Department
        {
            get { return _department; }
            set
            {
                if (_department != value)
                {
                    _department = value;
                    OnPropertyChanged(nameof(Department));
                }
            }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged(nameof(Address));
                }
            }
        }

        public Employee() { }

        public Employee(string employeeName, string address)
        {
            EmployeeName = employeeName;
            Address = address;
        }

        public Employee(string employeeName, DepartmentEnum department, string address)
        {
            EmployeeName = employeeName;
            Department = department;
            Address = address;
        }

        public Employee(int id, string employeeName, DepartmentEnum department, string address)
        {
            Id = id;
            EmployeeName = employeeName;
            Department = department;
            Address = address;
        }

        public Employee Clone()
        {
            return (Employee) MemberwiseClone();
        }

        public void CopyEmployee(Employee emp)
        {
            EmployeeName = emp.EmployeeName;
            Department = emp.Department;
            Address = emp.Address;
        }
    }
}
