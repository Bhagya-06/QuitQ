using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuitQ.API.DTOs.Request;
using QuitQ.API.DTOs.Response;
using QuitQ.API.Models;
using QuitQ.API.Services.Interfaces;

namespace QuitQ.API.Controllers
{
    [ApiController]
    [Route("api/offer")]
    public class OfferController : ControllerBase
    {
        private readonly IOfferService _service;

        public OfferController(IOfferService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateOfferRequest dto)
        {
            var offer = new Offer
            {
                Title = dto.Title,
                DiscountPercentage = dto.DiscountPercentage,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsActive = dto.IsActive ?? true
            };

            var result = await _service.CreateOffer(offer);

            return Ok(ApiResponse<object>.Success(result, "Offer created"));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(ApiResponse<object>.Success(await _service.GetAllOffers(), "Offers retrieved"));

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOfferById(int id)
        {
            var result = await _service.GetOfferById(id);
            return Ok(ApiResponse<object>.Success(result, "Offer retrieved"));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOffer(int id, CreateOfferRequest dto)
        {
            var offer = new Offer
            {
                Title = dto.Title,
                DiscountPercentage = dto.DiscountPercentage,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsActive = dto.IsActive
            };

            await _service.UpdateOffer(id, offer);

            return Ok(ApiResponse<object>.Success(null, "Offer updated"));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteOffer(id);
            return Ok(ApiResponse<object>.Success(null, "Offer deleted"));
        }
    }
}