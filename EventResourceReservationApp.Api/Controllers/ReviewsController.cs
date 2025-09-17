using EventResourceReservationApp.Api.Helpers;
using EventResourceReservationApp.Application.DTOs.Reviews;
using EventResourceReservationApp.Application.UseCases.Reviews;
using Microsoft.AspNetCore.Mvc;

namespace EventResourceReservationApp.Api.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    public class ReviewsController : Controller
    {
        private readonly CreateReviewUseCase _createUseCase;
        private readonly ReadAllReviewUseCase _ReadAllUseCase;
        private readonly ReadByIdReviewUseCase _readByIdUseCase;
        private readonly UpdateReviewUseCase _updateUseCase;
        private readonly DeleteReviewUseCase _deleteUseCase;
        public ReviewsController(
            CreateReviewUseCase createUseCase,
            ReadAllReviewUseCase readAllUseCase,
            ReadByIdReviewUseCase readByIdUseCase,
            UpdateReviewUseCase updateUseCase,
            DeleteReviewUseCase deleteUseCase
        )
        {
            _createUseCase = createUseCase;
            _ReadAllUseCase = readAllUseCase;
            _readByIdUseCase = readByIdUseCase;
            _updateUseCase = updateUseCase;
            _deleteUseCase = deleteUseCase;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReviewRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _createUseCase.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
            }
            return this.HandleOperationError(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("El Id proporcionado no es válido.");
            }
            var result = await _readByIdUseCase.ExecuteAsync(id);
            if(result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return this.HandleOperationError(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ReadReviewRequest request)
        {
            var result = await _ReadAllUseCase.ExecuteAsync(request);
            if(result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return this.HandleOperationError(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateReviewRequest request)
        {
            if(id == Guid.Empty || id != request.Id)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Inconsistencia de ID",
                    Detail = "El ID en la URL no coincide con el ID en el cuerpo de la solicitud."
                });
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }
            var result = await _updateUseCase.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return this.HandleOperationError(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if(id == Guid.Empty)
            {
                return BadRequest("El Id proporcionado no es válido.");
            }
            var result = await _deleteUseCase.ExecuteAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return this.HandleOperationError(result);
        }
    }
}
