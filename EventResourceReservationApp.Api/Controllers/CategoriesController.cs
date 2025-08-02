using EventResourceReservationApp.Application.DTOs.Categories;
using EventResourceReservationApp.Application.UseCases.Categories;
using Microsoft.AspNetCore.Mvc;

namespace EventResourceReservationApp.Api.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly CreateCategoryUseCase _createCategoryUseCase;
        private readonly UpdateCategoryUseCase _UpdateCategoryUseCase;
        public CategoriesController(
            CreateCategoryUseCase createCategoryUseCase,
            UpdateCategoryUseCase updateCategoryUseCase
            )
        {
            _createCategoryUseCase = createCategoryUseCase;
            _UpdateCategoryUseCase = updateCategoryUseCase;
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _createCategoryUseCase.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return Ok(new
                {
                    Message = result.Message,
                    Category = result.Data
                });
            }
            else
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Error en la operación",
                    Detail = result.Message
                });
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _UpdateCategoryUseCase.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return Ok(new
                {
                    Message = result.Message
                });
            }
            else
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Error en la operación",
                    Detail = result.Message
                });
            }
        }
    }
}
