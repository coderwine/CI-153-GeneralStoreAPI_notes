using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeneralStoreAPI.Data;
using Microsoft.AspNetCore.Mvc;
using GeneralStoreAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GeneralStoreAPI.Controllers
{
    [Route("[controller]")]
    // [ApiController]
    public class TransactionController : Controller
    {
        private GeneralStoreDBContext _db;
        public TransactionController(GeneralStoreDBContext db)
        {
            _db = db;
        }

        //! POST
        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromForm] TransactionEdit newTransaction)
        {
            Transaction transaction = new Transaction()
            {
                ProductId = newTransaction.ProductId,
                CustomerId = newTransaction.CustomerId,
                Quantity = newTransaction.Quantity,
            };

            //* NEED TO ACCOUNT FOR QUANTITY IN PRODUCTS TO REDUCE
            //* NEED TO TIE TO CUSTOMER
            Customer customer = await _db.Customers.FindAsync(transaction.CustomerId);
            Product product = await _db.Products.FindAsync(transaction.ProductId);

            transaction.Customer = customer;
            transaction.Product = product;

            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync();
            return Ok();
        }

        //! GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _db.Transactions.ToListAsync();
            return Ok(transactions);
        }

        //! PUT
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateTransaction([FromForm] TransactionEdit model, [FromRoute] int id)
        {
            var oldTrans = await _db.Transactions.FindAsync(id);

            if(oldTrans is null)
            {
                return NotFound();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            if(model.Quantity > 0)
            {
                oldTrans.Quantity = model.Quantity;
            }

            await _db.SaveChangesAsync();
            return Ok();
        }

        //! DELETE
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteTransaction([FromRoute] int id)
        {
            var transaction = await _db.Transactions.FindAsync(id);
            if(transaction is null)
            {
                return NotFound();
            }

            _db.Transactions.Remove(transaction);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}