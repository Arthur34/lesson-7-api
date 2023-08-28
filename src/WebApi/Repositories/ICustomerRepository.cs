using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repositories
{
    public interface ICustomerRepository
    {
        Task<long> CreateAsync(Customer customer);
        Task<Customer> Get(long id);
        Customer GetByName(string firstName, string lastName);
    }
}