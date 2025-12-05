using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.Repositories;
using Microsoft.Extensions.Logging;

namespace EventResourceReservationApp.Application.UseCases.Categories
{
    public class DeleteCategoryUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteCategoryUseCase> _logger;
        public DeleteCategoryUseCase(IUnitOfWork unitOfWork, ILogger<DeleteCategoryUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult> ExecuteAsync(int categoryId)
        {
            try
            {
                var deleteCategory = await _unitOfWork.Categories.GetByIdAsync(categoryId);
                if (deleteCategory == null)
                {
                    _logger.LogWarning("Fallo al eliminar: No se encontró una categoría con el ID '{CategoryId}'.", categoryId);
                    return OperationResult.Failure(
                            $"No se encontró una categoría con el ID '{categoryId}'.",
                            "NotFound",
                            "La operación de eliminación falló porque la categoría no existe."
                    );
                }
                //validar que no tenga recursos asociados
                //TODO: crear la validacion de busqueda de recursos asociados a la categoria, y solo eliminar si no tiene 
                // ningun recurso asociado
                await _unitOfWork.Categories.RemoveAsync(deleteCategory);
                await _unitOfWork.SaveAsync();
                return OperationResult.Success("Categoría eliminada exitosamente.");
            }
            catch (PersistenceException pEx)
            {
                _logger.LogError(pEx, "Fallo al eliminar la categoría debido a un error de persistencia.");
                return OperationResult.Failure("No se pudo eliminar la categoría de la base de datos.",
                    "PersistenceError",
                    "La operación de eliminación falló debido a un problema de almacenamiento de datos."
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error inesperado durante la eliminación de la categoría en el caso de uso.");
                return OperationResult.Failure("Ocurrió un error interno imprevisto.",
                    "UnexpectedError",
                    "La operación de eliminación falló debido a un problema inesperado."
                );
            }
        }
    }
}
