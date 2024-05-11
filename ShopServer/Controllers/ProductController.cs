using Microsoft.AspNetCore.Mvc;
using SharedModels.Models;
using SharedModels.Repository;

namespace ShopServer.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProduct _product) : ControllerBase {
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts(bool featured) {
            List<Product> products = await _product.GetProducts(featured);
            return Ok(products);
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product model) {
            var response = await _product.AddProduct(model);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response);
        }
        //[HttpPut]
        //public async Task<IActionResult> UpdateProduct(Product model) {
        //    var response = await _product.UpdateProduct(model);
        //    if (response.IsSuccess)
        //        return Ok(response);
        //    return BadRequest(response);
        //}
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteProduct(int id) {
        //    var response = await _product.DeleteProduct(id);
        //    if (response.IsSuccess)
        //        return Ok(response);
        //    return BadRequest(response);
        //}
    }
}
