using ProjetoEmpresa.Models;
using ProjetoEmpresa.Models.Repositories;
using ProjetoEmpresa.ViewModels.VMUtils;
using ProjetoEmpresa.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;

namespace ProjetoEmpresa.ViewModels
{
    public class MainWindowsVM : BaseNotifier
    {
        private PostgresDB postgresDB { get; set; }
        private ObservableCollection<Employee> _employeeList;
        private string _selectedFilter;        

        public IEnumerable<Employee> EmployeeList
        {
            get
            {
                if (SelectedFilter == "Todos")
                {
                    return _employeeList;
                }
                else
                {
                    return _employeeList.Where(employee => employee.Department == SelectedFilter);
                }
            }
        }

        private Employee _selectedEmployee;
        public Employee selectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                if (_selectedEmployee != value)
                {
                    _selectedEmployee = value;
                    OnPropertyChanged(nameof(selectedEmployee));
                }
            }
        }

        public string SelectedFilter
        {
            get { return _selectedFilter; }
            set
            {
                _selectedFilter = value;
                OnPropertyChanged(nameof(EmployeeList));
            }
        }        

        public ICommand Add { get; private set; }
        public ICommand Edit { get; private set; }
        public ICommand Remove { get; private set; }

        public MainWindowsVM() {
            postgresDB = new PostgresDB();
            try
            {
                _employeeList = postgresDB.GetEmployees();
            }
            catch (Exception)
            {
                _employeeList = new ObservableCollection<Employee>();
            }

            InitializeCommands();
        }

        public void InitializeCommands()
        {
            Add = new RelayCommand((object _) =>
            {
                Employee newEmployee = new Employee ();

                EmployeeCreationEditView screen = new EmployeeCreationEditView();
                screen.DataContext = newEmployee;
                bool? verifica = screen.ShowDialog();

                if (verifica.HasValue && verifica.Value)
                {
                    if (postgresDB.InsertEmployee(newEmployee) == 1) {
                        _employeeList.Add(newEmployee);
                        _employeeList = postgresDB.GetEmployees();
                        SelectedFilter = newEmployee.Department;
                        OnPropertyChanged(nameof(SelectedFilter));
                        OnPropertyChanged(nameof(EmployeeList));
                        selectedEmployee = newEmployee;
                    } 
                    else
                    {
                        Console.WriteLine("ERRO!");
                    }                    
                }
            });

            Edit = new RelayCommand((object _) =>
            {
                EmployeeCreationEditView screen = new EmployeeCreationEditView();

                Employee employeeToUpdate = selectedEmployee.Clone();
                screen.DataContext = employeeToUpdate;
                bool? verifica = screen.ShowDialog();

                if (verifica.HasValue && verifica.Value)
                {
                    if (postgresDB.UpdateEmployee(employeeToUpdate) == 1)
                    {
                        selectedEmployee.CopyEmployee(employeeToUpdate);
                        _employeeList = postgresDB.GetEmployees();
                        OnPropertyChanged(nameof(EmployeeList));
                    }
                    else
                    {
                        Console.WriteLine("ERRO!");
                    }
                }
            }, (object _) =>
            {
                return selectedEmployee != null;
            });

            Remove = new RelayCommand((object _) => {

                if(postgresDB.DeleteEmployee(selectedEmployee.Id) == 1)
                {
                    _employeeList.Remove(selectedEmployee);
                    _employeeList = postgresDB.GetEmployees();
                    OnPropertyChanged(nameof(EmployeeList));
                    selectedEmployee = _employeeList.FirstOrDefault();
                }

            }, (object _) =>
            {
                return selectedEmployee != null;
            });
        }
    }
}
