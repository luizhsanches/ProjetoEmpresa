using ProjetoEmpresa.Models;
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
                    return _employeeList.Where(employee => employee.Department.ToString() == SelectedFilter);
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
            _employeeList = new ObservableCollection<Employee>();

            InitializeCommands();
        }

        public void InitializeCommands()
        {
            Add = new RelayCommand((object _) =>
            {
                Employee employee = new Employee ();

                EmployeeCreationEditView screen = new EmployeeCreationEditView();
                screen.DataContext = employee;
                bool? verifica = screen.ShowDialog();

                if (verifica.HasValue && verifica.Value)
                {
                    _employeeList.Add(employee);
                    OnPropertyChanged(nameof (EmployeeList));
                    selectedEmployee = employee;
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
                    selectedEmployee.CopyEmployee(employeeToUpdate);
                    OnPropertyChanged(nameof(EmployeeList));
                }
            }, (object _) =>
            {
                return selectedEmployee != null;
            });

            Remove = new RelayCommand((object _) => {
                _employeeList.Remove(selectedEmployee);
                OnPropertyChanged(nameof(EmployeeList));
                selectedEmployee = _employeeList.FirstOrDefault();

            }, (object _) =>
            {
                return selectedEmployee != null;
            });
        }
    }
}
