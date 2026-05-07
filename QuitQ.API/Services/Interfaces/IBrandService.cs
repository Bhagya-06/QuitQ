using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using QuitQ.API.DTOs.Response;
using QuitQ.API.Models;

namespace QuitQ.API.Services.Interfaces
{
    public interface IBrandService
    {
        Task<List<BrandResponse>> GetAll();
        Task Add([FromBody] string name);
        Task Update(int id, [FromBody] string name);
        Task Delete(int id);
    }
}
