using EventResourceReservationApp.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace EventResourceReservationApp.Api.Helpers
{
    public static class ResultHelper
    {
        public static IActionResult HandleOperationError(this ControllerBase controller, OperationResult result)
        {
            return result.ErrorCode switch
            {
                "Conflict" => controller.Conflict(new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Conflicto",
                    Detail = result.Message
                }),
                "NotFound" => controller.NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "No encontrado",
                    Detail = result.Message
                }),
                "InvalidInput" => controller.BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Solicitud inválida",
                    Detail = result.Message
                }),
                _ => controller.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Error interno del servidor",
                    Detail = "Ocurrió un error inesperado. Por favor, inténtelo de nuevo más tarde."
                }),
            };
        }
        public static IActionResult HandleOperationError<T>(this ControllerBase controller, OperationResult<T> result)
        {
            return result.ErrorCode switch
            {
                "Conflict" => controller.Conflict(new ProblemDetails
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Conflicto",
                    Detail = result.Message
                }),
                "NotFound" => controller.NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "No encontrado",
                    Detail = result.Message
                }),
                "InvalidInput" => controller.BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Solicitud inválida",
                    Detail = result.Message
                }),
                _ => controller.StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Error interno del servidor",
                    Detail = "Ocurrió un error inesperado. Por favor, inténtelo de nuevo más tarde."
                }),
            };
        }
    }
}
