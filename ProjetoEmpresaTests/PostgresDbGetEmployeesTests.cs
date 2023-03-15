using ProjetoEmpresa.Models.Repositories;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ProjetoEmpresa.Models;
using System.Collections.ObjectModel;

namespace ProjetoEmpresaTests
{
    [TestFixture]
    public class PostgresDbGetEmployeesTests
    {
        private readonly string createTableString = "CREATE TABLE employee (id serial NOT NULL, employee_name VARCHAR(255) NOT NULL, department VARCHAR(255) NOT NULL, address VARCHAR(255) NOT NULL); ALTER TABLE employee ADD CONSTRAINT employee_pkey PRIMARY KEY (id)";
        private readonly string dropTableString = "DROP TABLE employee;";
        private readonly string connectionString = "Server=localhost;Port=5432;Database=projeto_empresa_test;User Id=postgres;Password=1234;Integrated Security=true;";
        private IDatabase database;

        [SetUp]
        public void SetUp()
        {
            database = new PostgresDB(true);

            using (NpgsqlConnection connection = new(connectionString))
            {
                connection.Open();
                using (NpgsqlCommand command = new())
                {
                    command.Connection = connection;
                    command.CommandText = createTableString;
                    command.ExecuteNonQuery();
                }
            }
        }

        [TearDown]
        public void TearDown()
        {
            using (NpgsqlConnection connection = new(connectionString))
            {
                connection.Open();
                using (NpgsqlCommand command = new())
                {
                    command.Connection = connection;
                    command.CommandText = dropTableString;
                    command.ExecuteNonQuery();
                }
            }
        }


        [Test]
        public void GetEmployees_ReturnsAllEmployees_WhenEmployeesExist()
        {
            // Arrange
            ObservableCollection<Employee> expectedEmployees = new()
            {
                new Employee { EmployeeName = "Employee 1", Department = DepartmentEnum.Financeiro, Address = "Rua X" },
                new Employee { EmployeeName = "Employee 2", Department = DepartmentEnum.Gerencia, Address = "Rua Y" }
            };

            database.InsertEmployee(expectedEmployees[0]);
            database.InsertEmployee(expectedEmployees[1]);

            // Act
            ObservableCollection<Employee> actualEmployees = database.GetEmployees();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(CompareDirectors(actualEmployees[0], expectedEmployees[0]), Is.EqualTo(true));
                Assert.That(CompareDirectors(actualEmployees[1], expectedEmployees[1]), Is.EqualTo(true));
            });
        }

        private bool CompareDirectors(Employee actualEmployee, Employee expectedEmployee)
        {
            return
              actualEmployee.EmployeeName == expectedEmployee.EmployeeName &&
              actualEmployee.Department == expectedEmployee.Department &&
              actualEmployee.Address == expectedEmployee.Address;
        }

    }
}
