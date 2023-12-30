using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;

namespace CHERRY.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _ICategoryService;
        public CategoryController(ICategoryService ICategoryService)
        {
            _ICategoryService = ICategoryService;
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CategoryCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _ICategoryService.CreateAsync(model);
            if (!result)
            {
                return BadRequest(new { Message = "Error creating Category" });
            }

            return Ok(new { Message = "Category created successfully" });
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            var classifies = await _ICategoryService.GetAllAsync();
            return Ok(classifies);
        }

        [HttpGet]
        [Route("getallactive")]
        public async Task<IActionResult> GetAllActive()
        {
            var classifies = await _ICategoryService.GetAllActiveAsync();
            return Ok(classifies);
        }

        [HttpGet("GetByID/{ID}")]
        public async Task<IActionResult> GetByID(Guid ID)
        {
            var classify = await _ICategoryService.GetByIDAsync(ID);
            if (classify == null)
            {
                return NotFound();
            }

            return Ok(classify);
        }

        [HttpPut("{ID}")]
        public async Task<IActionResult> Update(Guid ID, [FromBody] CategoryUpdateVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _ICategoryService.UpdateAsync(ID, model);
            if (!result)
            {
                return BadRequest(new { Message = "Error updating" });
            }

            return Ok(new { Message = "Updated successfully" });
        }

        [HttpDelete("{ID}")]
        public async Task<IActionResult> DeleteClassify(Guid ID, [FromQuery] string IDUserdelete)
        {
            var result = await _ICategoryService.RemoveAsync(ID, IDUserdelete);
            if (!result)
            {
                return BadRequest(new { Message = "Error deleting Category" });
            }

            return Ok(new { Message = "Category deleted successfully" });
        }
    }
}
