using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Net;
using System.Collections.ObjectModel;
using System.Collections;
using System.Data.Common;

namespace ProjetoEmpresa.Models.Repositories
{
    public class PostgresDB : IDatabase
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;

        public PostgresDB()
        {
        }

        public ObservableCollection<Employee> GetEmployees()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                string selectQuery = "SELECT * FROM employee";
                using (NpgsqlCommand command = new NpgsqlCommand(selectQuery, conn))
                {
                    try
                    {
                        conn.Open();
                        ObservableCollection<Employee> employees = new ObservableCollection<Employee>();
                        NpgsqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Employee emp = new Employee(
                                (int)reader["id"],
                                (string)reader["employee_name"],
                                (string)reader["department"],
                                (string)reader["address"]
                            );

                            employees.Add(emp);
                        }

                        reader.Close();

                        return employees;
                    }
                    catch (NpgsqlException npg)
                    {
                        throw new Exception("Ocorreu um erro ao acessar o banco de dados." + npg.Message);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Erro ao carregar funcionarios: " + ex.Message);
                    }
                }
            }
        }

        public void InsertEmployee(Employee newEmployee)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                using (NpgsqlCommand insertCommand = new NpgsqlCommand())
                {
                    try
                    {
                        conn.Open();
                        insertCommand.Connection = conn;
                        insertCommand.CommandText = "INSERT INTO employee (employee_name, department, address) VALUES (@employee_name, @department, @address)";
                        
                        insertCommand.Parameters.AddWithValue("@employee_name", newEmployee.EmployeeName);
                        insertCommand.Parameters.AddWithValue("@department", newEmployee.Department);
                        insertCommand.Parameters.AddWithValue("@address", newEmployee.Address);

                        insertCommand.ExecuteNonQuery();
                    }
                    catch (InvalidOperationException)
                    {
                        throw new Exception("Dados invalidos.");
                    }
                    catch (NpgsqlException npg)
                    {
                        throw new Exception("Ocorreu um erro ao acessar o banco de dados." + npg.Message);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Erro ao inserir funcionario: " + ex.Message);
                    }                    
                }
            }
        }

        public void UpdateEmployee(Employee employeeToUpdate)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                using (NpgsqlCommand updateCommand = new NpgsqlCommand())
                {
                    try
                    {
                        conn.Open();
                        updateCommand.Connection = conn;
                        updateCommand.CommandText = "UPDATE employee SET employee_name = @employee_name, department = @department, address = @address WHERE id = @id";
                        
                        updateCommand.Parameters.AddWithValue("@employee_name", employeeToUpdate.EmployeeName);
                        updateCommand.Parameters.AddWithValue("@department", employeeToUpdate.Department);
                        updateCommand.Parameters.AddWithValue("@address", employeeToUpdate.Address);
                        updateCommand.Parameters.AddWithValue("@id", employeeToUpdate.Id);

                        updateCommand.ExecuteNonQuery();
                    }
                    catch (InvalidOperationException)
                    {
                        throw new Exception("Dados invalidos.");
                    }
                    catch (NpgsqlException npg)
                    {
                        throw new Exception("Ocorreu um erro ao acessar o banco de dados. " + npg.Message);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Erro ao atualizar funcionario: " + ex.Message);
                    }
                }
            }
        }

        public void DeleteEmployee(int employeeId)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                using (NpgsqlCommand deleteCommand = new NpgsqlCommand())
                {
                    try
                    {
                        conn.Open();
                        deleteCommand.Connection = conn;
                        deleteCommand.CommandText = "DELETE FROM employee WHERE id = @id";

                        deleteCommand.Parameters.AddWithValue("id", employeeId);

                        deleteCommand.ExecuteNonQuery();
                    }
                    catch (InvalidOperationException)
                    {
                        throw new Exception("Dados invalidos.");
                    }
                    catch (NpgsqlException npg)
                    {
                        throw new Exception("Ocorreu um erro ao acessar o banco de dados." + npg.Message);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Erro ao remover funcionario: " + ex.Message);
                    }
                }
            }
        }
    }
}
