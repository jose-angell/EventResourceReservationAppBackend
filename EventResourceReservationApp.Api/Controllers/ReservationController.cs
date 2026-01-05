using EventResourceReservationApp.Api.Helpers;
using EventResourceReservationApp.Application.DTOs.Reservations;
using EventResourceReservationApp.Application.UseCases.Reservations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace EventResourceReservationApp.Api.Controllers
{
    [ApiController]
    [Route("api/reservations")]
    public class ReservationController : ControllerBase
    {
        private readonly CreateReservationUseCase _create;
        private readonly ReadAllReservationUseCase _readAll;
        private readonly ReadByIdReservationUseCase _readById;
        private readonly DeleteReservationUseCase _delete;
        private readonly UpdateReservationUseCase _update;
        private readonly UpdateStatusReservationUseCase _updateStatus;
        private readonly UpdateTransationReservationUseCase _updateTransaction;

        public ReservationController(
            CreateReservationUseCase create,
            ReadAllReservationUseCase readAll,
            ReadByIdReservationUseCase readById,
            DeleteReservationUseCase delete,
            UpdateReservationUseCase update,
            UpdateStatusReservationUseCase updateStatus,
            UpdateTransationReservationUseCase updateTransaction)
        {
            _create = create;
            _readAll = readAll;
            _readById = readById;
            _delete = delete;
            _update = update;
            _updateStatus = updateStatus;
            _updateTransaction = updateTransaction;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> ReadByIdReservation([FromRoute] Guid id)
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
            var result = await _readById.ExecuteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return this.HandleOperationError(result);
        }
        [HttpGet]
        public async Task<IActionResult> ReadAllReservations(ReadAllReservationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _readAll.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return this.HandleOperationError(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _create.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(ReadByIdReservation), new { id = result.Data.Id, result.Data });
            }
            return this.HandleOperationError(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservation([FromRoute] Guid id, [FromBody] UpdateReservationRequest request)
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
            if (id != request.Id)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Inconsistencia de ID",
                    Detail = "El ID en la URL no coincide con el ID en el cuerpo de la solicitud."
                });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _update.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return this.HandleOperationError(result);
        }
        [HttpPut("transation/{id}")]
        public async Task<IActionResult> UpdateTransationReservation([FromRoute] Guid id, [FromBody] UpdateTransationReservationRequest request)
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
            if (id != request.Id)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Inconsistencia de ID",
                    Detail = "El ID en la URL no coincide con el ID en el cuerpo de la solicitud."
                });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _updateTransaction.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return this.HandleOperationError(result);
        }
        [HttpPut("status/{id}")]
        public async Task<IActionResult> UpdateStatusReservation([FromRoute] Guid id, [FromBody] UpdateStatusReservationRequest request)
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
            if (id != request.Id)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Inconsistencia de ID",
                    Detail = "El ID en la URL no coincide con el ID en el cuerpo de la solicitud."
                });
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _updateStatus.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return this.HandleOperationError(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation([FromRoute] Guid id)
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
            var result = await _delete.ExecuteAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return this.HandleOperationError(result);
        }
    }
}
