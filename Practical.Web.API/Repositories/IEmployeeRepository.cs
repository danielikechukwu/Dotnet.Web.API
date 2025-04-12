using Practical.Web.API.Models;

namespace Practical.Web.API.Repositories
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAll();

        Employee? GetById(int id);

        void Add(Employee employee);

        void Update(Employee employee);

        void Delete(int Id);

        bool Exists(int Id);

    }
}
