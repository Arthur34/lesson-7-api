using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("customers")]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository repo;

        public CustomerController(ICustomerRepository repository)
        {
            repo = repository;
        }

        /// <summary>
        /// Считать покупателя из БД по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:long}")]
        public async Task<ActionResult> GetCustomerAsync([FromRoute] long id)
        {
            var customer = await repo.Get(id);

            if (customer != null)
                return Ok(customer);

            return NotFound();
        }

        /// <summary>
        /// Создать покупателя в БД
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]   
        public async Task<ActionResult> CreateCustomerAsync([FromBody] Customer customer)
        {
            if (customer == null)
                return BadRequest();

            // проверяем, существует ли с таким именем
            var isExisting = (repo.GetByName(customer.Firstname, customer.Lastname) != null);

            if (isExisting)
                return new StatusCodeResult(StatusCodes.Status409Conflict);

            long id = await repo.CreateAsync(customer);

            return new ObjectResult(id) { StatusCode = StatusCodes.Status200OK };
        }
    }
}