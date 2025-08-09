using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Categories;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Domain;

namespace EventResourceReservationApp.Application.UseCases.Categories
{
    public class CreateCategoryUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateCategoryUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<OperationResult<CategoryResponse>> ExecuteAsync(CreateCategoryRequest request)
        {
            var existingCategory = await _unitOfWork.Categories.GetFirstOrDefaultAsync(c => c.Name == request.Name);
            if (existingCategory != null)
            {
                //TODO: _logger.LogWarning("Fallo al crear categoría: Ya existe una categoría con el nombre '{CategoryName}'.", request.Name);
                return OperationResult<CategoryResponse>.Failure(
                    $"Ya existe una categoría con el nombre '{request.Name}'.",
                    "Conflict",
                    $"La operación de actualización falló debido a una duplicación de nombre '{request.Name}'."
                );
            }
            Category newCategory;
            try
            {
                newCategory = new Category(request.Name, request.Description, request.CreatedByUserId);

                await _unitOfWork.Categories.AddAsync(newCategory);
                await _unitOfWork.SaveAsync();
                var categoryDto = new CategoryResponse
                {
                    Id = newCategory.Id,
                    Name = newCategory.Name,
                    Description = newCategory.Description,
                    CreatedAt = newCategory.CreatedAt,
                    CreatedByUserId = newCategory.CreatedByUserId
                };
                return OperationResult<CategoryResponse>.Success(categoryDto, "Categoría creada exitosamente.");
            }
            catch (ArgumentException argEx)
            {
                //TODO: _logger.LogWarning(argEx, "Fallo al crear categoría debido a argumentos inválidos: {ErrorMessage}", argEx.Message);
                return OperationResult<CategoryResponse>.Failure(
                    "La operación de creación falló debido a una entrada inválida.",
                    "InvalidInput",
                    argEx.Message
                );
            }
            catch (PersistenceException pEx)
            {
                //TODO: _logger.LogError(pEx, "Fallo al crear categoría debido a un error de persistencia.");
                return OperationResult<CategoryResponse>.Failure("No se pudo guardar la categoría en la base de datos.",
                    "PersistenceError",
                    "La operación de creación falló debido a un problema de almacenamiento de datos."
                );
            }
            catch (Exception exc)
            {
                //TODO: _logger.LogError(ex, "Ocurrió un error inesperado durante la creación de la categoría en el caso de uso.");
                return OperationResult<CategoryResponse>.Failure("Ocurrió un error interno imprevisto.",
                    "UnexpectedError",
                    "La operación de creación falló debido a un problema inesperado."
                );
            }
        }
    }
}
