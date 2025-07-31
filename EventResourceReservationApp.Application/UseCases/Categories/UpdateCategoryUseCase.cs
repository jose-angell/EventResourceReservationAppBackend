using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Categories;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Domain;

namespace EventResourceReservationApp.Application.UseCases.Categories
{
    public class UpdateCategoryUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateCategoryUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<OperationResult> ExecuteAsync(UpdateCategoryRequest request)
        {
            Category updateCategory;

            updateCategory = await _unitOfWork.Categories.GetByIdAsync(request.Id);
            if (updateCategory == null)
            {
                //TODO: _logger.LogWarning("Fallo al actualizar: No se encontró una categoría con el ID '{CategoryId}'.", request.Id);
                return OperationResult.Failure(
                        $"No se encontró una categoría con el ID '{request.Id}'.",
                        "La operación de actualización falló porque la categoría no existe."
                );
            }
            try
            { 
                //Regla de Negocio: Verificar si el nuevo nombre ya existe en OTRA categoría
                if (!string.Equals(updateCategory.Name, request.Name, StringComparison.OrdinalIgnoreCase))
                {
                    var existingCategoryWithSameName = await _unitOfWork.Categories.GetFirstOrDefaultAsync(c => c.Name == request.Name);
                    if (existingCategoryWithSameName != null)
                    {
                        ///TODO: _logger.LogWarning("Fallo al actualizar categoría: Ya existe otra categoría con el nombre '{CategoryName}'.", request.Name);
                        return OperationResult.Failure(
                            $"Ya existe una categoría con el nombre '{request.Name}'.",
                            "La operación de actualización falló debido a una duplicación de nombre."
                        );
                    }
                }
                updateCategory.Update(request.Name, request.Description);
            
                await _unitOfWork.Categories.UpdateAsync(updateCategory);
                await _unitOfWork.SaveAsync();
                return OperationResult.Success("Categoría actualizada exitosamente.");
            }
            catch (ArgumentException argEx)
            {
                //TODO: _logger.LogWarning(argEx, "Fallo al actualizar categoría debido a argumentos inválidos: {ErrorMessage}", argEx.Message);
                return OperationResult.Failure(argEx.Message,
                    "La operación de actualización falló debido a una entrada inválida."
                );
            }
            catch (PersistenceException pEx)
            {
                //TODO: _logger.LogError(pEx, "Fallo al actualizar la categoría debido a un error de persistencia.");
                return OperationResult.Failure("No se pudo guardar los cambios de la categoría en la base de datos.",
                    "La operación de actualización falló debido a un problema de almacenamiento de datos."
                );
            }
            catch (Exception ex)
            {
                //TODO: _logger.LogError(ex, "Ocurrió un error inesperado durante la actualización de la categoría en el caso de uso.");
                return OperationResult.Failure("Ocurrió un error interno imprevisto.",
                    "La operación de actualización falló debido a un problema inesperado."
                );
            }

        }
    }
}
