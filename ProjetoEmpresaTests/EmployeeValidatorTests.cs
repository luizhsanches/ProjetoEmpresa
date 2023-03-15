using ProjetoEmpresa.Models;

namespace ProjetoEmpresaTests
{
    [TestFixture]
    public class EmployeeValidatorTests
    {
        private IEmployeeValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new EmployeeValidator();
        }

        [Test]
        public void ValidateEmployeeNameIsNull_ThrowsArgumentException()
        {
            Employee employee = new Employee
            {
                EmployeeName = null,
                Department = DepartmentEnum.Tecnologia,
                Address = "Rua X"
            };

            TestDelegate action = () => validator.Validate(employee);

            Assert.Throws<ArgumentException>(action);
        }

        [Test]
        public void ValidateEmployeeNameIsLessThanThreeCharacters_ThrowsArgumentException()
        {
            Employee employee = new Employee
            {
                EmployeeName = "Lu",
                Department = DepartmentEnum.Tecnologia,
                Address = "Rua X"
            };

            TestDelegate action = () => validator.Validate(employee);

            Assert.Throws<ArgumentException>(action);
        }

        [Test]
        public void ValidateAddressIsNull_ThrowsArgumentException()
        {
            Employee employee = new Employee
            {
                EmployeeName = "Luiz",
                Department = DepartmentEnum.Tecnologia,
                Address = null
            };

            TestDelegate action = () => validator.Validate(employee);

            Assert.Throws<ArgumentException>(action);
        }

        [Test]
        public void ValidateAddressIsLessThanThreeCharacters_ThrowsArgumentException()
        {
            Employee employee = new Employee
            {
                EmployeeName = "Luiz",
                Department = DepartmentEnum.Tecnologia,
                Address = "Ru"
            };

            TestDelegate action = () => validator.Validate(employee);

            Assert.Throws<ArgumentException>(action);
        }
    }
}