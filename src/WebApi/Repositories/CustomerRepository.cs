using System.Linq;
using System.Threading.Tasks;
using WebApi.DbProvider;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext db;

        public CustomerRepository(AppDbContext db)
        {
            this.db = db;
        }

        public async Task<Customer> Get(long id)
        {
            return await db.Customers.FindAsync(id);
        }

        public Customer GetByName(string firstName, string lastName)
        {
            return db.Customers.FirstOrDefault(c => c.Firstname == firstName && c.Lastname == lastName);
        }

        public async Task<long> CreateAsync(Customer customer)
        {
            db.Customers.Add(customer);
            await db.SaveChangesAsync();

            return customer.Id;
        }
    }
}