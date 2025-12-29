using EventResourceReservationApp.Api.Helpers;
using EventResourceReservationApp.Application.DTOs.ReservationDetails;
using EventResourceReservationApp.Application.UseCases.ReservationDetails;
using Microsoft.AspNetCore.Mvc;

namespace EventResourceReservationApp.Api.Controllers
{
    [ApiController]
    [Route("api/reservationDetails")]
    public class ReservationDetailController : ControllerBase
    {
        private readonly ReadAllReservationDetailUseCase _readAll;
        private readonly ReadByIdReservationDetailUseCase _readById;
        private readonly CreateReservationDetailUseCase _create;
        private readonly UpdateReservationDetailUseCase _update;
        private readonly DeleteReservationDetailUseCase _delete;
        public ReservationDetailController(
            ReadAllReservationDetailUseCase readAll,
            ReadByIdReservationDetailUseCase readById,
            CreateReservationDetailUseCase create,
            UpdateReservationDetailUseCase update,
            DeleteReservationDetailUseCase delete)
        {
            _readAll = readAll;
            _readById = readById;
            _create = create;
            _update = update;
            _delete = delete;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> ReadByIdReservationDetail([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Entrada inválida",
                    Detail = "El ID del detalle de la reserva es necesario."
                });
            }
            var result = await _readById.ExecuteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return this.HandleOperationError(result);
        }
        [HttpGet("all-details/{id}")]
        public async Task<IActionResult> ReadAllReservationDetails([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Entrada inválida",
                    Detail = "El ID de la reserva es necesario."
                });
            }
            var result = await _readAll.ExecuteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return this.HandleOperationError(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateReservationDetail([FromBody] CreateReservationDetailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _create.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(ReadByIdReservationDetail), new { id = result.Data.Id }, result.Data);
            }
            return this.HandleOperationError(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservationDetail([FromRoute] Guid id, [FromBody] UpdateReservationDetailRequest request)
        {

            if (id == Guid.Empty)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Entrada inválida",
                    Detail = "El ID del detalle de la reserva es necesario."
                });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != request.Id)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Entrada inválida",
                    Detail = "El ID del detalle de la reserva en la ruta y en el modelo deben ser iguales."
                });
            }
            request.Id = id;
            var result = await _update.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return this.HandleOperationError(result);
        }
        [HttpDelete("{id}")]    
        public async Task<IActionResult> DeleteReservationDetail([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Entrada inválida",
                    Detail = "El ID del detalle de la reserva es necesario."
                });
            }
            var result = await _delete.ExecuteAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return this.HandleOperationError(result);
        }
    }
}
