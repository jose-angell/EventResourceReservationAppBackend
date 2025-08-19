using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Categories;
using EventResourceReservationApp.Application.Repositories;
using Microsoft.Extensions.Logging;

namespace EventResourceReservationApp.Application.UseCases.Categories
{
    public class ListAllCategoryUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ListAllCategoryUseCase> _logger;
        public ListAllCategoryUseCase(IUnitOfWork unitOfWork, ILogger<ListAllCategoryUseCase> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<OperationResult<IEnumerable<CategoryListItemResponse>>> ExecuteAsync()
        {
            try
            {

                var categories = await _unitOfWork.Categories.GetAllAsync();
                var categoryList = categories.Select(categories => new CategoryListItemResponse
                {
                    Id = categories.Id,
                    Name = categories.Name
                });
                return OperationResult<IEnumerable<CategoryListItemResponse>>.Success(categoryList, "Categorías obtenidas exitosamente.");
            }
            catch (PersistenceException pEx)
            {
                _logger.LogError(pEx, "Fallo al obtener las categorías debido a un error de persistencia.");
                return OperationResult<IEnumerable<CategoryListItemResponse>>.Failure(
                    "No se pudieron obtener las categorías de la base de datos.",
                    "PersistenceError",
                    "La operación de lectura falló debido a un problema de almacenamiento de datos."
                );
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error inesperado durante la obtención de las categorías en el caso de uso.");
                return OperationResult<IEnumerable<CategoryListItemResponse>>.Failure(
                    "Ocurrió un error interno imprevisto.",
                    "UnexpectedError",
                    "La operación de lectura falló debido a un problema inesperado."
                );
            }
        }
    }
}
