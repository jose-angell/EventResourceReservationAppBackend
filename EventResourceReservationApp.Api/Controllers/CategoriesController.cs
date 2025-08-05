using EventResourceReservationApp.Application.Common;
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
            OperationResult<CategoryResponse> result = await _createCategoryUseCase.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(ReadByIdCategory), new { id = result.Data.Id }, result.Data);
            }
           
            if (result.ErrorCode == "DuplicateName")
            {
                return Conflict(new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Conflicto al crear la categoría",
                    Detail = result.Message
                });
            }
            if(result.ErrorCode == "InvalidInput")
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Entrada inválida",
                    Detail = result.Message
                });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Error inesperado al crear la categoría.",
                Detail = "Ocurrió un error inesperado en el servidor."
            });
        }
        [HttpGet]
        public async Task<IActionResult> ReadAllCategory([FromQuery] ReadAllCategoryRequest request)
        {
            var result = await _readAllCategory.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Error inesperado al consultar las categorías.",
                Detail = "Ocurrió un error inesperado en el servidor."
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> ReadByIdCategory([FromRoute] int id)
        {
            
            if (id <= 0)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Entrada inválida",
                    Detail = "El ID de la categoría debe ser mayor que cero."
                });
            }
            var result = await _readByIdCategory.ExecuteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            if (result.ErrorCode == "NotFound")
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Recurso no encontrado",
                    Detail = result.Message
                });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Error inesperado al consultar la categoría.",
                Detail = "Ocurrió un error inesperado en el servidor."
            });
        }
        [HttpGet("list")]
        public async Task<IActionResult> ListAllCategory()
        {
            var result = await _listAllCategory.ExecuteAsync();
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Error inesperado al consultar las categorías.",
                Detail = "Ocurrió un error inesperado en el servidor."
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
                return NoContent();
            }
            if (result.ErrorCode == "DuplicateName")
            {
                return Conflict(new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Conflicto al crear la categoría",
                    Detail = result.Message
                });
            }
            if (result.ErrorCode == "InvalidInput")
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Entrada inválida",
                    Detail = result.Message
                });
            }
            if (result.ErrorCode == "NotFound")
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Recurso no encontrado",
                    Detail = result.Message
                });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Error inesperado al actualizar la categoría.",
                Detail = "Ocurrió un error inesperado en el servidor."
            });

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Entrada inválida",
                    Detail = "El ID de la categoría debe ser mayor que cero."
                });
            }
            var result = await _deleteCategory.ExecuteAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            if (result.ErrorCode == "NotFound")
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Recurso no encontrado",
                    Detail = result.Message
                });
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Error inesperado al eliminar la categoría.",
                Detail = "Ocurrió un error inesperado en el servidor."
            });
        }
    }
}
