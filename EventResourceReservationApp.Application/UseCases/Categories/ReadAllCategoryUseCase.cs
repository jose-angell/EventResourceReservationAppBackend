using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Categories;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Domain;
using System.Linq.Expressions;

namespace EventResourceReservationApp.Application.UseCases.Categories
{
    public class ReadAllCategoryUseCase
    {
        private readonly IUnitOfWork _UnitOfWork;
        public ReadAllCategoryUseCase(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }
        public async Task<OperationResult<IEnumerable<CategoryResponse>>> ExecuteAsync(ReadAllCategoryRequest request)
        {
            try
            {
                Expression<Func<Category, bool>>? filter = null;
                Func<IQueryable<Category>, IOrderedQueryable<Category>>? orderBy = null;
                if (!string.IsNullOrEmpty(request.NameFilter))
                {
                    filter = c => c.Name.Contains(request.NameFilter);
                }
                if (request.CreatedByUserIdFilter.HasValue && request.CreatedByUserIdFilter != Guid.Empty)
                {
                    if (filter != null)
                    {
                        Expression<Func<Category, bool>> newFilter = c => c.CreatedByUserId == request.CreatedByUserIdFilter;
                        var parameter = filter.Parameters.Single();
                        var body = Expression.AndAlso(filter.Body, Expression.Invoke(newFilter, parameter));
                        filter = Expression.Lambda<Func<Category, bool>>(body, parameter);
                    }
                    else
                    {
                        filter = c => c.CreatedByUserId == request.CreatedByUserIdFilter;
                    }
                }
                if (!string.IsNullOrEmpty(request.OrderBy))
                {
                    if (request.OrderBy.Equals("Name_asc", StringComparison.OrdinalIgnoreCase))
                    {
                        orderBy = q => q.OrderBy(c => c.Name);
                    }
                    else if (request.OrderBy.Equals("CreatedAt_asc", StringComparison.OrdinalIgnoreCase))
                    {
                        orderBy = q => q.OrderBy(c => c.CreatedAt);
                    }
                }
                var categories = await _UnitOfWork.Categories.GetAllAsync(filter, orderBy);
                var categoryResponses = categories.Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    CreatedAt = c.CreatedAt,
                    CreatedByUserId = c.CreatedByUserId
                });
                return OperationResult<IEnumerable<CategoryResponse>>.Success(categoryResponses, "Categorías obtenidas exitosamente.");
            }
            catch (PersistenceException pEx)
            {
                //TODO: _logger.LogError(pEx, "Fallo al obtener las categorías debido a un error de persistencia.");
                return OperationResult<IEnumerable<CategoryResponse>>.Failure(
                    "No se pudieron obtener las categorías de la base de datos.",
                    "PersistenceError",
                    "La operación de lectura falló debido a un problema de almacenamiento de datos."
                );
            }
            catch (Exception ex)
            {
                //TODO: _logger.LogError(ex, "Ocurrió un error inesperado durante la obtención de las categorías en el caso de uso.");
                return OperationResult<IEnumerable<CategoryResponse>>.Failure(
                    "Ocurrió un error interno imprevisto.",
                    "UnexpectedError",
                    "La operación de lectura falló debido a un problema inesperado."
                );
            }
        }

    }
}
