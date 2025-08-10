using EventResourceReservationApp.Application.Common;
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
    public class ListAllCategoryUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _mockIunitOfWork;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly ListAllCategoryUseCase _useCase;
        public ListAllCategoryUseCaseTests()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockIunitOfWork = new Mock<IUnitOfWork>();
            _mockIunitOfWork.Setup(u => u.Categories).Returns(_mockCategoryRepository.Object);
            _useCase = new ListAllCategoryUseCase(_mockIunitOfWork.Object);
        }
        [Fact]
        public async Task ExecuteAsync_ReturnsListOfCategories()
        {
            // Arrange
            var expectedCategories = new List<Category>
            {
                new Category { Id = 1, Name = "Category 1", Description = "Description 1" },
                new Category { Id = 2, Name = "Category 2", Description = "Description 2" }
            };
            _mockCategoryRepository.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Func<IQueryable<Category>, IOrderedQueryable<Category>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(expectedCategories);
            // Act
            var result = await _useCase.ExecuteAsync();
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Data.Count());
        }
        [Fact]
        public async Task ExecuteAsync_WhenNoCategories_ReturnsEmptyList()
        {
            // Arrange
            _mockCategoryRepository.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Func<IQueryable<Category>, IOrderedQueryable<Category>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(new List<Category>());
            // Act
            var result = await _useCase.ExecuteAsync();
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Data);
        }
        [Fact]
        public async Task ExecuteAsync_WithPersistenceException_ReturnsFailure()
        {
            // Arrange
            _mockCategoryRepository.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<Func<IQueryable<Category>, IOrderedQueryable<Category>>>(),
                It.IsAny<string>()))
                .ThrowsAsync(new PersistenceException("Database error"));
            // Act
            var result = await _useCase.ExecuteAsync();
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("PersistenceError", result.ErrorCode);
            Assert.Equal("La operación de lectura falló debido a un problema de almacenamiento de datos.", result.Message);
        }
    }
}
