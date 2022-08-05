using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeneralStoreAPI.Data; // Allows us access to the DBContext file.
using Microsoft.AspNetCore.Mvc; // Allows us for the [ApiController]
using GeneralStoreAPI.Models;
using Microsoft.EntityFrameworkCore; // Allows us access to the method ToListAsync()

namespace GeneralStoreAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private GeneralStoreDBContext _db;
        public ProductController(GeneralStoreDBContext db)
        {
            _db = db;
        }

        //! POST
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm]ProductEdit newProduct) 
        {
            Product product = new Product() {
                Name = newProduct.Name,
                Price = newProduct.Price,
                QuantityInStock = newProduct.Quantity,
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return Ok();
        }

        //! GET
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _db.Products.ToListAsync();
            return Ok(products);
        }

        //! PUT
        [HttpPut]
        [Route("{id}")] // Needed to pull from the URL - injected into the [FromRoute]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductEdit model, [FromRoute] int id)
        {
            //Est. old object
            var oldProduct = await _db.Products.FindAsync(id);

            //Make sure old object isn't null
            if(oldProduct is null)
            {
                return NotFound();
            }

            //Make sure that the information being provided is good to be set in the object.
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            if(!string.IsNullOrEmpty(model.Name))
            {
                oldProduct.Name = model.Name;
            }

            if(!double.IsNaN(model.Price))
            {
                oldProduct.Price = model.Price;
            }

            if(model.Quantity > 0)
            {
                oldProduct.QuantityInStock = model.Quantity;
            }

            await _db.SaveChangesAsync();
            return Ok();
        }

        //! DELETE
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            var product = await _db.Products.FindAsync(id);
            if(product == null)
            {
                return NotFound();
            }

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}