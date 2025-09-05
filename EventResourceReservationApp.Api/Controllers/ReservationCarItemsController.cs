using EventResourceReservationApp.Api.Helpers;
using EventResourceReservationApp.Application.DTOs.ReservationCarItems;
using EventResourceReservationApp.Application.UseCases.ReservationCarItems;
using Microsoft.AspNetCore.Mvc;

namespace EventResourceReservationApp.Api.Controllers
{
    [ApiController]
    [Route("api/reservation-car-items")]
    public class ReservationCarItemsController : ControllerBase
    {
        private readonly CreateReservationCarItemUseCase _CreateUseCase;
        private readonly ReadAllReservationCarItemUseCase _ReadAllUseCase;
        private readonly UpdateReservationCarItemUseCase _UpdateUseCase;
        private readonly DeleteReservationCarItemUseCase _DeleteUseCase;
        public ReservationCarItemsController(
            CreateReservationCarItemUseCase createUseCase,
            ReadAllReservationCarItemUseCase readAllUseCase,
            UpdateReservationCarItemUseCase updateUseCase,
            DeleteReservationCarItemUseCase deleteUseCase)
        {
            _CreateUseCase = createUseCase;
            _ReadAllUseCase = readAllUseCase;
            _UpdateUseCase = updateUseCase;
            _DeleteUseCase = deleteUseCase;
        }
        [HttpPost]
        public async Task<IActionResult> CreateReservationCarItem([FromBody] CreateReservationCarItemRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _CreateUseCase.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(ReadAllReservationCarItems), new { id = result.Data.ClientId }, result.Data);
            }
            return this.HandleOperationError(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> ReadAllReservationCarItems([FromRoute] Guid clientId)
        {
            if (clientId == Guid.Empty)
            {
                return BadRequest("El Id del Carrito de reservas no puede ser vacío.");
            }
            var result = await _ReadAllUseCase.ExecuteAsync(clientId);
            if(result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return this.HandleOperationError(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservationCarItem([FromRoute] Guid id, [FromBody] UpdateReservationCarItemRequest request)
        {
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
                    Detail = "El Id del elemento a actualizar no coincide con el Id en la ruta."
                });
            }
            var result = await _UpdateUseCase.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return this.HandleOperationError(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservationCarItem([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Entrada inválida",
                    Detail = "El ID del elemento no se encontro."
                });
            }
            var result = await _DeleteUseCase.ExecuteAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return this.HandleOperationError(result);
        }
    }
}
