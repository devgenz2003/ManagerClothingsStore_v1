﻿using CHERRY.BUS.Services._1_Interfaces;
using CHERRY.BUS.ViewModels.Brand;
using CHERRY.BUS.ViewModels.Material;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CHERRY.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandServices _IBrandServices;

        public BrandController(IBrandServices IBrandServices)
        {
            _IBrandServices = IBrandServices;
        }

        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAll()
        {
            var materials = await _IBrandServices.GetAllAsync();
            return Ok(materials);
        }
        [HttpGet]
        [Route("getallactive")]
        public async Task<IActionResult> GetAllActive()
        {
            var materials = await _IBrandServices.GetAllActiveAsync();
            return Ok(materials);
        }
        [HttpGet]
        [Route("GetByID/{ID}")]
        public async Task<IActionResult> GetByID(Guid ID)
        {
            var material = await _IBrandServices.GetByIDAsync(ID);
            if (material == null)
            {
                return NotFound();
            }
            return Ok(material);
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(BrandCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _IBrandServices.CreateAsync(request);
            if (success)
            {
                return Ok(success);
            }
            return BadRequest("Failed to create material");
        }
        [HttpPut("update/{ID}")]
        public async Task<IActionResult> Update(Guid ID, BrandUpdateVM request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _IBrandServices.UpdateAsync(ID, request);
            if (success)
            {
                return NoContent();
            }
            return BadRequest("Failed to update material");
        }
        [HttpDelete("{ID}/{IDUserDelete}")]
        public async Task<IActionResult> Remove(Guid ID, string IDUserDelete)
        {
            var success = await _IBrandServices.RemoveAsync(ID, IDUserDelete);
            if (success)
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
