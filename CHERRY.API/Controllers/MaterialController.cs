using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.Material;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CHERRY.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialServices _materialServices;

        public MaterialController(IMaterialServices materialServices)
        {
            _materialServices = materialServices;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            var materials = await _materialServices.GetAllAsync();
            return Ok(materials);
        }

        [HttpGet]
        [Route("getallactive")]
        public async Task<IActionResult> GetAllActive()
        {
            var materials = await _materialServices.GetAllActiveAsync();
            return Ok(materials);
        }

        [HttpGet]
        [Route("GetByID/{ID}")]
        public async Task<IActionResult> GetByID(Guid ID)
        {
            var obj = await _materialServices.GetByIDAsync(ID);
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(MaterialCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _materialServices.CreateAsync(request);
            if (success)
            {
                return CreatedAtAction(nameof(GetByID), new { id = request.ID }, request);
            }
            return BadRequest("Failed to create material");
        }

        [HttpPut("{ID}")]
        public async Task<IActionResult> UpdateMaterial(Guid ID, MaterialUpdateVM request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _materialServices.UpdateAsync(ID, request);
            if (success)
            {
                return NoContent();
            }
            return BadRequest("Failed to update material");
        }

        [HttpDelete("{ID}")]
        public async Task<IActionResult> DeleteMaterial(Guid ID, string IDUserDelete)
        {
            var success = await _materialServices.RemoveAsync(ID, IDUserDelete);
            if (success)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
