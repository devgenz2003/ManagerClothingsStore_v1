using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.Services._2_Implements;
using CHERRY.BUS.ViewModels.Brand;
using CHERRY.BUS.ViewModels.Colors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CHERRY.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        private readonly IColorsService _IColorsService;

        public ColorsController(IColorsService IColorsService)
        {
            _IColorsService = IColorsService;
        }
        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            var materials = await _IColorsService.GetAllAsync();
            return Ok(materials);
        }
        [HttpGet]
        [Route("getallactive")]
        public async Task<IActionResult> GetAllActive()
        {
            var materials = await _IColorsService.GetAllActiveAsync();
            return Ok(materials);
        }
        [HttpGet]
        [Route("GetByID/{ID}")]
        public async Task<IActionResult> GetByID(Guid ID)
        {
            var material = await _IColorsService.GetByIDAsync(ID);
            if (material == null)
            {
                return NotFound();
            }
            return Ok(material);
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(ColorsCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _IColorsService.CreateAsync(request);
            if (success)
            {
                return Ok(success);
            }
            return BadRequest("Failed to create Colors");
        }
        [HttpPut("update/{ID}")]
        public async Task<IActionResult> Update(Guid ID, ColorsUpdateVM request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _IColorsService.UpdateAsync(ID, request);
            if (success)
            {
                return NoContent();
            }
            return BadRequest("Failed to update Colors");
        }
        [HttpDelete("{ID}/{IDUserDelete}")]
        public async Task<IActionResult> Remove(Guid ID, string IDUserDelete)
        {
            var success = await _IColorsService.RemoveAsync(ID, IDUserDelete);
            if (success)
            {
                return NoContent();
            }
            return NotFound();
        }

    }
}
