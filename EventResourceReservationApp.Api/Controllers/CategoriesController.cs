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
        private readonly ReadAllCategoryUseCase _readAllCategory;
        private readonly ReadByIdCategoryUseCase _readByIdCategory;
        private readonly ListAllCategoryUseCase _listAllCategory;
        private readonly DeleteCategoryUseCase _deleteCategory;
        public CategoriesController(
            CreateCategoryUseCase createCategoryUseCase,
            UpdateCategoryUseCase updateCategoryUseCase,
            ReadAllCategoryUseCase readAllCategoryUseCase,
            ReadByIdCategoryUseCase readByIdCategoryUseCase,
            ListAllCategoryUseCase listAllCategoryUseCase,
            DeleteCategoryUseCase deleteCategoryUseCase
            )
        {
            _createCategoryUseCase = createCategoryUseCase;
            _UpdateCategoryUseCase = updateCategoryUseCase;
            _readAllCategory = readAllCategoryUseCase;
            _readByIdCategory = readByIdCategoryUseCase;
            _listAllCategory = listAllCategoryUseCase;
            _deleteCategory = deleteCategoryUseCase;
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
        [HttpGet]
        public async Task<IActionResult> ReadAllCategory([FromQuery] ReadAllCategoryRequest request)
        {
            var result = await _readAllCategory.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return Ok(new
                {
                    Message = result.Message,
                    Categories = result.Data
                });
            }
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Error en la operación",
                Detail = result.Message
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> ReadByIdCategory(int id = 0)
        {
            if (id <= 0)
            {
                return BadRequest(id);
            }
            var result = await _readByIdCategory.ExecuteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(new
                {
                    Message = result.Message,
                    Category = result.Data
                });
            }
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Error en la operación",
                Detail = result.Message
            });
        }
        [HttpGet("list")]
        public async Task<IActionResult> ListAllCategory()
        {
            var result = await _listAllCategory.ExecuteAsync();
            if (result.IsSuccess)
            {
                return Ok(new
                {
                    Message = result.Message,
                    Categories = result.Data
                });
            }
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Error en la operación",
                Detail = result.Message
            });
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID de la categoría debe ser mayor que cero.");
            }
            var result = await _deleteCategory.ExecuteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(new
                {
                    Message = "Categoría eliminada exitosamente."
                });
            }
            return BadRequest(new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Error en la operación",
                Detail = result.Message
            });
        }
    }
}
