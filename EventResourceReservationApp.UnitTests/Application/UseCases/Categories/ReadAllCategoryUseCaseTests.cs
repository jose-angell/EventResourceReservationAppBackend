using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Categories;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Application.UseCases.Categories;
using EventResourceReservationApp.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.UnitTests.Application.UseCases.Categories
{
    public class ReadAllCategoryUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _mockIunitOfWork;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly ReadAllCategoryUseCase _useCase;
        public ReadAllCategoryUseCaseTests()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockIunitOfWork = new Mock<IUnitOfWork>();
            _mockIunitOfWork.Setup(u => u.Categories).Returns(_mockCategoryRepository.Object);
            _useCase = new ReadAllCategoryUseCase(_mockIunitOfWork.Object);
        }
        // Datos de prueba para simular la base de datos
        private List<Category> GetTestCategories()
        {
            return new List<Category>
            {
                new Category { Id = 1, Name = "Electronics", Description = "Devices", CreatedByUserId = new Guid("11111111-1111-1111-1111-111111111111"), CreatedAt = new DateTime(2025, 1, 1) },
                new Category { Id = 2, Name = "Books", Description = "Reading materials", CreatedByUserId = new Guid("22222222-2222-2222-2222-222222222222"), CreatedAt = new DateTime(2025, 1, 3) },
                new Category { Id = 3, Name = "Home Appliances", Description = "Kitchen and laundry", CreatedByUserId = new Guid("11111111-1111-1111-1111-111111111111"), CreatedAt = new DateTime(2025, 1, 2) },
                new Category { Id = 4, Name = "Computers", Description = "PCs and laptops", CreatedByUserId = new Guid("33333333-3333-3333-3333-333333333333"), CreatedAt = new DateTime(2025, 1, 4) }
            };
        }
        [Fact]
        public async Task ExecuteAsync_WithoutFiltersOrOrdering_ReturnsAllCategories()
        {
            // Arrange
            var categories = GetTestCategories();
            var request = new ReadAllCategoryRequest();
            // Mockeamos GetAllAsync para que aplique los filtros a nuestra lista de prueba
            _mockCategoryRepository.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Func<IQueryable<Category>, IOrderedQueryable<Category>>>(),
                It.IsAny<string>()))
                .ReturnsAsync((Expression<Func<Category, bool>> filter, Func<IQueryable<Category>, IOrderedQueryable<Category>> orderBy, string include) =>
                {
                    var query = categories.AsQueryable();
                    if (filter != null)
                    {
                        query = query.Where(filter);
                    }
                    if (orderBy != null)
                    {
                        return orderBy(query).ToList();
                    }
                    return query.ToList();
                });
            // Act
            var result = await _useCase.ExecuteAsync(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(4, result.Data.Count());

        }
        [Fact]
        public async Task ExecuteAsync_WithFilterByName_ReturnsFilteredCategories()
        {
            // Arrange
            var categories = GetTestCategories();
            var request = new ReadAllCategoryRequest { NameFilter = "Computers" };
            _mockCategoryRepository.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Func<IQueryable<Category>, IOrderedQueryable<Category>>>(),
                It.IsAny<string>()))
                .ReturnsAsync((Expression<Func<Category, bool>> filter, Func<IQueryable<Category>, IOrderedQueryable<Category>> orderBy, string include) =>
                {
                    var query = categories.AsQueryable();
                    if (filter != null)
                    {
                        query = query.Where(filter);
                    }
                    return query.ToList();
                });

            // Act
            var result = await _useCase.ExecuteAsync(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Data);
            Assert.Equal("Computers", result.Data.First().Name);
        }
        [Fact]
        public async Task ExecuteAsync_WithFilterByUserId_ReturnsFilteredCategories()
        {
            // Arrange 
            var categories = GetTestCategories();
            var userId = new Guid("11111111-1111-1111-1111-111111111111");
            var request = new ReadAllCategoryRequest { CreatedByUserIdFilter = userId };

            _mockCategoryRepository.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Func<IQueryable<Category>, IOrderedQueryable<Category>>>(),
                It.IsAny<string>()))
                .ReturnsAsync((Expression<Func<Category, bool>> filter, Func<IQueryable<Category>, IOrderedQueryable<Category>> orderBy, string include) =>
                {
                    var query = categories.AsQueryable();
                    if (filter != null)
                    {
                        query = query.Where(filter);
                    }
                    return query.ToList();
                });
            // Act
            var result = await _useCase.ExecuteAsync(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count());
        }
        [Fact]
        public async Task ExecuteAsync_WithOrderingByNameAsc_ReturnsOrderedCategories() 
        {
            // Arrange
            var categories = GetTestCategories();
            var request = new ReadAllCategoryRequest { OrderBy = "Name_asc" };
            _mockCategoryRepository.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Func<IQueryable<Category>, IOrderedQueryable<Category>>>(),
                It.IsAny<string>()))
                .ReturnsAsync((Expression<Func<Category, bool>> filter, Func<IQueryable<Category>, IOrderedQueryable<Category>> orderBy, string include) =>
                {
                    var query = categories.AsQueryable();
                    if (orderBy != null)
                    {
                        return orderBy(query).ToList();
                    }
                    return query.ToList();
                });

            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.True(result.IsSuccess);
            var categoryNames = result.Data.Select(c => c.Name).ToList();
            Assert.Equal(new[] { "Books", "Computers", "Electronics", "Home Appliances" }, categoryNames);
        }
        [Fact]
        public async Task ExecuteAsync_WithPersistenceException_ReturnsFailure()
        {
            // Arrange
            var categories = GetTestCategories();
            var request = new ReadAllCategoryRequest();
            _mockCategoryRepository.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Func<IQueryable<Category>, IOrderedQueryable<Category>>>(),
                It.IsAny<string>()))
                .ThrowsAsync(new PersistenceException("Simulated database error."));
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("La operación de lectura falló debido a un problema de almacenamiento de datos.", result.Message);
        }
    }
}
