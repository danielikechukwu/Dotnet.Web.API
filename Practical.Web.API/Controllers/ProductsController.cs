﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Practical.Web.API.Models;

namespace Practical.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        // In-memory list to store products (for demonstration purposes)
        // In Real-time, we will use database
        private static List<Product> _products = new List<Product> {
            new Product { Id = 1, Name = "Laptop", Price = 1000.00m, Category = "Electronics" },
            new Product { Id = 2, Name = "Desktop", Price = 2000.00m, Category = "Electronics" },
            new Product { Id = 3, Name = "Mobile", Price = 300.00m, Category = "Electronics" },
            // Additional products can be added here
        };

        [Route("All")]
        [HttpGet]

        //Response for successful operation
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]

        //Response for 500 error
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(5));

                return Ok(_products);

            }
            catch (Exception ex)
            {
                // Return 500 Internal Server Error in case of an exception
                return StatusCode(500, "Internal server error");
            }
        }

        //GET: api/products
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            return Ok(_products);
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = _products.FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return NotFound(new { Message = $"Product with ID {id} not found"});
            }
            return Ok(product);
        }

        //POST: api/products
        [HttpPost]
        public ActionResult<Product> PostProduct([FromBody] Product product)
        {
            // Basic ID assignment logic (for demonstration)
            product.Id = _products.Max(p => p.Id) + 1;
            _products.Add(product);
            return CreatedAtAction(nameof(GetProduct), new {id = product.Id}, product);

        }

        //Handling lists of productions.
        [Route("CreateListProducts")]
        [HttpPost]
        public ActionResult<Order> CreateListProducts([FromBody] Order order)
        {
            // Handle multiple products
            // For demonstration, let's return the products back
            return Ok(order);
        }

        //PUT: api/products/{id}
        [HttpPut("{id}")]
        public IActionResult PutProduct(int id, [FromBody] Product updatedProduct)
        {
            if (id != updatedProduct.Id)
            {
                return BadRequest(new { Message = $"ID mismatch between route and body." });
            }

            var existingProduct = _products.FirstOrDefault(p => p.Id == id);

            if (existingProduct == null)
            {
                return NotFound(new { Message = $"Product with ID {id} not found" });
            }

            //Update the product properties
            existingProduct.Name = updatedProduct.Name;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.Category = updatedProduct.Category;

            // In a real application, persist changes to the database here

            return NoContent();

        }

        //DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound(new { Message = $"Product with ID {id} Not Found" });
            }

            _products.Remove(product);

            // In a real application, remove the product from the database here

            return NoContent();
        }
    }
}
