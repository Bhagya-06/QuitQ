using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using QuitQ.API.DTOs.Response;
using QuitQ.API.Services.Interfaces;

namespace QuitQ.API.Controllers
{
    [ApiController]
    [Route("api/brand")]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _service;

        public BrandController(IBrandService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await _service.GetAll();
            return Ok(ApiResponse<object>.Success(data));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] string name)
        {
            await _service.Add(name);
            return Ok(ApiResponse<string>.Success(null, "Brand created"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] string name)
        {
            await _service.Update(id, name);
            return Ok(ApiResponse<string>.Success(null, "Brand updated"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return Ok(ApiResponse<string>.Success(null, "Brand deleted"));
        }
    }
}
