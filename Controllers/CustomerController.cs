using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeneralStoreAPI.Data;
using GeneralStoreAPI.Models;
using Microsoft.AspNetCore.Mvc; // Allows for the Controller class to be imported
using Microsoft.EntityFrameworkCore;

namespace GeneralStoreAPI.Controllers
{
    [ApiController]
    [Route("[controller]")] // Defines our routes in the URL
    public class CustomerController : Controller
    {
        private GeneralStoreDBContext _db;
        public CustomerController(GeneralStoreDBContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromForm]CustomerEdit newCustomer)
        {
            Customer customer = new Customer()
            {
                Name = newCustomer.Name,
                Email = newCustomer.Email,  
            };

            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _db.Customers.ToListAsync(); // This method comes from Microsoft.EntityFrameworkCore
            
            return Ok(customers);
        }
    }
}