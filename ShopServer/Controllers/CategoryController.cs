using Microsoft.AspNetCore.Mvc;
using SharedModels.Models;
using ShopServer.Repository;

namespace ShopServer.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategory _Category) : ControllerBase {
        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetCategories() {
            List<Category> Categories = await _Category.GetCategories();
            return Ok(Categories);
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(Category model) {
            var response = await _Category.AddCategory(model);
            if (response.IsSuccess)
                return Ok(response);
            return BadRequest(response);
        }
        //[HttpPut]
        //public async Task<IActionResult> UpdateCategory(Category model) {
        //    var response = await _Category.UpdateCategory(model);
        //    if (response.IsSuccess)
        //        return Ok(response);
        //    return BadRequest(response);
        //}
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCategory(int id) {
        //    var response = await _Category.DeleteCategory(id);
        //    if (response.IsSuccess)
        //        return Ok(response);
        //    return BadRequest(response);
        //}
    }
}
