using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.Brand;
using CHERRY.BUS.ViewModels.Sizes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CHERRY.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizesController : ControllerBase
    {
        private readonly ISizesService _ISizesService;

        public SizesController(ISizesService ISizesService)
        {
            _ISizesService = ISizesService;
        }
        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            var materials = await _ISizesService.GetAllAsync();
            return Ok(materials);
        }
        [HttpGet]
        [Route("getallactive")]
        public async Task<IActionResult> GetAllActive()
        {
            var materials = await _ISizesService.GetAllActiveAsync();
            return Ok(materials);
        }
        [HttpGet]
        [Route("GetByID/{ID}")]
        public async Task<IActionResult> GetByID(Guid ID)
        {
            var material = await _ISizesService.GetByIDAsync(ID);
            if (material == null)
            {
                return NotFound();
            }
            return Ok(material);
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(SizesCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _ISizesService.CreateAsync(request);
            if (success)
            {
                return Ok(success);
            }
            return BadRequest("Failed to create Sizes");
        }
        [HttpPut("update/{ID}")]
        public async Task<IActionResult> Update(Guid ID, SizesUpdateVM request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _ISizesService.UpdateAsync(ID, request);
            if (success)
            {
                return NoContent();
            }
            return BadRequest("Failed to update Sizes");
        }
        [HttpDelete("{ID}/{IDUserDelete}")]
        public async Task<IActionResult> Remove(Guid ID, string IDUserDelete)
        {
            var success = await _ISizesService.RemoveAsync(ID, IDUserDelete);
            if (success)
            {
                return NoContent();
            }
            return NotFound();
        }

    }
}
