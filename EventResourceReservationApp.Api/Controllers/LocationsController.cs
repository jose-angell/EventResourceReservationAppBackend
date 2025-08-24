using EventResourceReservationApp.Api.Helpers;
using EventResourceReservationApp.Application.DTOs.Loctions;
using EventResourceReservationApp.Application.UseCases.Locations;
using Microsoft.AspNetCore.Mvc;

namespace EventResourceReservationApp.Api.Controllers
{
    [ApiController]
    [Route("api/locations")]
    public class LocationsController : ControllerBase
    {
        private readonly CreateLocationUseCase _createUseCase;
        private readonly UpdateLocationUseCase _updateUseCase;
        private readonly DeleteLocationUseCase _deleteUseCase;
        private readonly ReadAllLocationUseCase _readAllUseCase;
        private readonly ReadByIdLocationUseCase _readByIdUseCase;
        public LocationsController(
            CreateLocationUseCase createUseCase,
            UpdateLocationUseCase updateUseCase,
            DeleteLocationUseCase deleteUseCase,
            ReadAllLocationUseCase readAllUseCase,
            ReadByIdLocationUseCase readByIdUseCase
            )
        {
            _createUseCase = createUseCase;
            _updateUseCase = updateUseCase;
            _deleteUseCase = deleteUseCase;
            _readAllUseCase = readAllUseCase;
            _readByIdUseCase = readByIdUseCase;
        }
        [HttpPost]
        public async Task<IActionResult> CreateLocation([FromBody] CreateLocationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _createUseCase.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(ReadByIdLocation), new { id = result.Data.Id }, result.Data);
            }
            return this.HandleOperationError(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocation([FromRoute] int id, [FromBody] UpdateLocationRequest request)
        {
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
            var result = await _updateUseCase.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return this.HandleOperationError(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ReadByIdLocation([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest("El ID debe ser un número entero positivo.");
            }
            var result = await _readByIdUseCase.ExecuteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return this.HandleOperationError(result);
        }
        [HttpGet]
        public async Task<IActionResult> ReadAllLocations([FromQuery] ReadAllLocationRequest request)
        {
            var result = await _readAllUseCase.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return this.HandleOperationError(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Entrada inválida",
                    Detail = "El ID de la ubicacion debe ser mayor que cero."
                });
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
