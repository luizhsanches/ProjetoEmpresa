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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;

namespace ProjetoEmpresa.ViewModels
{
    public class MainWindowsVM : BaseNotifier
    {
        private readonly IDatabase database;
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
            database = new PostgresDB();

            try
            {
                _employeeList = database.GetEmployees();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    try
                    {
                        database.InsertEmployee(newEmployee);

                        _employeeList.Add(newEmployee);
                        _employeeList = database.GetEmployees();
                        SelectedFilter = newEmployee.Department;

                        OnPropertyChanged(nameof(SelectedFilter));
                        OnPropertyChanged(nameof(EmployeeList));

                        selectedEmployee = newEmployee;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    try
                    {
                        database.UpdateEmployee(employeeToUpdate);
                        selectedEmployee.CopyEmployee(employeeToUpdate);
                        _employeeList = database.GetEmployees();
                        OnPropertyChanged(nameof(EmployeeList));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }, (object _) =>
            {
                return selectedEmployee != null;
            });

            Remove = new RelayCommand((object _) => {

                try
                {
                    database.DeleteEmployee(selectedEmployee.Id);

                    _employeeList.Remove(selectedEmployee);
                    _employeeList = database.GetEmployees();

                    OnPropertyChanged(nameof(EmployeeList));
                    selectedEmployee = _employeeList.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }, (object _) =>
            {
                return selectedEmployee != null;
            });
        }
    }
}
