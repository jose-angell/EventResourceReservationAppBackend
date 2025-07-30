using EventResourceReservationApp.Application.DTOs.Categories;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EventResourceReservationApp.Application.UseCases.Categories
{
    public class CreateCategoryUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateCategoryUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<CategoryResponse> ExecuteAsync(CreateCategoryRequest request)
        {
            var existingCategory = await _unitOfWork.Categories.GetFirstOrDefaultAsync(c => c.Name == request.Name);
            if (existingCategory != null)
            {
                throw new ArgumentException($"Ya existe una categoría con el nombre '{request.Name}'.", nameof(request.Name));
            }

            var newCategory = new Category(request.Name, request.Description, request.CreatedByUserId);

            await _unitOfWork.Categories.AddAsync(newCategory);
            await _unitOfWork.SaveAsync();
            return new CategoryResponse
            {
                Id = newCategory.Id,
                Name = newCategory.Name,
                Description = newCategory.Description,
                CreatedAt = newCategory.CreatedAt,
                CreatedByUserId = newCategory.CreatedByUserId
            };
        }
    }
}
