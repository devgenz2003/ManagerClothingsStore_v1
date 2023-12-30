using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.Brand;
using CHERRY.BUS.ViewModels.Review;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CHERRY.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _IReviewService;

        public ReviewController(IReviewService IReviewService)
        {
            _IReviewService = IReviewService;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            var materials = await _IReviewService.GetAllAsync();
            return Ok(materials);
        }

        [HttpGet]
        [Route("getallactive")]
        public async Task<IActionResult> GetAllActive()
        {
            var materials = await _IReviewService.GetAllActiveAsync();
            return Ok(materials);
        }

        [HttpGet]
        [Route("GetByID/{ID}")]
        public async Task<IActionResult> GetByID(Guid ID)
        {
            var material = await _IReviewService.GetByIDAsync(ID);
            if (material == null)
            {
                return NotFound();
            }
            return Ok(material);
        }
        [HttpGet]
        [Route("GetByVariant/{IDVariant}")]
        public async Task<IActionResult> GetByVariant(Guid IDVariant)
        {
            var material = await _IReviewService.GetByVariant(IDVariant);
            if (material == null)
            {
                return NotFound();
            }
            return Ok(material);
        }
        [HttpGet]
        [Route("GetByUser/{IDUser}")]
        public async Task<IActionResult> GetByUser(string IDUser)
        {
            var material = await _IReviewService.GetByUser(IDUser);
            if (material == null)
            {
                return NotFound();
            }
            return Ok(material);
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromForm] ReviewCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _IReviewService.CreateAsync(request);
            if (success)
            {
                return Ok(success);
            }
            return BadRequest("Failed to create");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, ReviewUpdateVM request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _IReviewService.UpdateAsync(id, request);
            if (success)
            {
                return NoContent();
            }
            return BadRequest("Failed to update material");
        }

        [HttpDelete("{ID}/{IDUserDelete}")]
        public async Task<IActionResult> Remove(Guid ID, string IDUserDelete)
        {
            var success = await _IReviewService.RemoveAsync(ID, IDUserDelete);
            if (success)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
