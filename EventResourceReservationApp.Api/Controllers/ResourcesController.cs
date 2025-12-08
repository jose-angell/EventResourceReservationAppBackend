using EventResourceReservationApp.Api.Helpers;
using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Resources;
using EventResourceReservationApp.Application.UseCases.Resources;
using Microsoft.AspNetCore.Mvc;

namespace EventResourceReservationApp.Api.Controllers
{
    [ApiController]
    [Route("api/resources")]
    public class ResourcesController : ControllerBase
    {
        private readonly CreateResourceUseCase _create;
        private readonly UpdateResourceUseCase _update;
        private readonly DeleteResourceUseCase _delete;
        private readonly ReadByIdResourceUseCase _readById;
        private readonly ReadByIdAndDateRangeResourceUseCase _readByIdAndDateRange;
        private readonly ReadAllResourceUseCase _readAll;

        public ResourcesController(
            CreateResourceUseCase create,
            UpdateResourceUseCase update,
            DeleteResourceUseCase delete,
            ReadByIdResourceUseCase readById,
            ReadByIdAndDateRangeResourceUseCase readByIdAndDateRange,
            ReadAllResourceUseCase readAll)
        {
            _create = create;
            _update = update;
            _delete = delete;
            _readById = readById;
            _readByIdAndDateRange = readByIdAndDateRange;
            _readAll = readAll;
        }
        [HttpPost]
        public async Task<IActionResult> CreateResource([FromBody] CreateResourceRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            OperationResult<ResourceResponse> result = await _create.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(ReadByIdResource), new { id = result.Data.Id }, result.Data);
            }
            return this.HandleOperationError(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResource([FromRoute] Guid id, [FromBody] UpdateResourceRequest request)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Entrada inválida",
                    Detail = "El ID del recurso es necesario."
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResource([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Entrada inválida",
                    Detail = "El ID del recurso es necesario."
                });
            }
            var result = await _delete.ExecuteAsync(id);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return this.HandleOperationError(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> ReadByIdResource([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Entrada inválida",
                    Detail = "El ID del recurso es necesario."
                });
            }
            var result  = await _readById.ExecuteAsync(id);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return this.HandleOperationError(result);
        }
        [HttpGet("DateRange/{id}")]
        public async Task<IActionResult> ReadByIdAndDateRangeResource([FromRoute] Guid id, ReadByIdAndDateRangeResourceRequest request)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Entrada inválida",
                    Detail = "El ID del recurso es necesario."
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
            var result = await _readByIdAndDateRange.ExecuteAsync(request);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return this.HandleOperationError(result);
        }
        [HttpGet]
        public async Task<IActionResult> ReadAllResource([FromBody] ReadAllResourceRequest request)
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
    }
}
