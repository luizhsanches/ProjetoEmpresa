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

namespace ProjetoEmpresa.Models.Repositories
{
    public class PostgresDB
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;

        public List<Employee> employeeList { get; set;}

        private NpgsqlConnection conn;

        public PostgresDB()
        {
            conn = new NpgsqlConnection(connectionString);
        }

        public ObservableCollection<Employee> GetEmployees()
        {
            ObservableCollection<Employee> employees = new ObservableCollection<Employee>();

            try
            {
                conn.Open();
                string selectQuery = "SELECT * FROM employee";

                NpgsqlCommand selectCommand = new NpgsqlCommand(selectQuery, conn);
                NpgsqlDataReader reader = selectCommand.ExecuteReader();

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
            catch (NpgsqlException)
            {
                throw new Exception("Ocorreu um erro ao acessar o banco de dados.");
            }
            catch (Exception ex) 
            {
                throw new Exception("Falha ao carregar funcionarios: " + ex.Message);
            }
            finally { conn.Close(); }            
        }

        public void InsertEmployee(Employee newEmployee)
        {
            try
            {
                conn.Open();
                string insertQuery = "INSERT INTO employee (employee_name, department, address) VALUES (@employee_name, @department, @address)";

                NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, conn);

                insertCommand.Parameters.AddWithValue("@employee_name", newEmployee.EmployeeName);
                insertCommand.Parameters.AddWithValue("@department", newEmployee.Department);
                insertCommand.Parameters.AddWithValue("@address", newEmployee.Address);

                insertCommand.ExecuteNonQuery();
            }
            catch (InvalidOperationException)
            {
                throw new Exception("Dados invalidos.");
            }
            catch (NpgsqlException)
            {
                throw new Exception("Ocorreu um erro ao acessar o banco de dados.");
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao inserir funcionario: " + ex.Message);
            }
            finally { conn.Close(); }
        }

        public void UpdateEmployee(Employee employeeToUpdate)
        {
            try
            {
                conn.Open();
                string updateQuery = "UPDATE employee SET employee_name = @employee_name, department = @department, address = @address WHERE id = @id";

                NpgsqlCommand updateCommand = new NpgsqlCommand(updateQuery, conn);

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
            catch (NpgsqlException)
            {
                throw new Exception("Ocorreu um erro ao acessar o banco de dados.");
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao atualizar funcionario: " + ex.Message);
            }
            finally { conn.Close(); }
        }

        public void DeleteEmployee(int employeeId)
        {
            try
            {
                conn.Open();
                string deleteQuery = "DELETE FROM employee WHERE id = @id";

                NpgsqlCommand deleteCommand = new NpgsqlCommand(deleteQuery, conn);

                deleteCommand.Parameters.AddWithValue("id", employeeId);

                deleteCommand.ExecuteNonQuery();
            }
            catch (NpgsqlException)
            {
                throw new Exception("Ocorreu um erro ao acessar o banco de dados.");
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao remover funcionario: " + ex.Message);
            }
            finally { conn.Close(); }
        }
    }
}
