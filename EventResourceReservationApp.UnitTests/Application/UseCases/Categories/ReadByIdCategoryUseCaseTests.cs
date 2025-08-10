using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Application.UseCases.Categories;
using EventResourceReservationApp.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.UnitTests.Application.UseCases.Categories
{
    public class ReadByIdCategoryUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _mockIunitOfWork;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly ReadByIdCategoryUseCase _useCase;
        public ReadByIdCategoryUseCaseTests()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockIunitOfWork = new Mock<IUnitOfWork>();
            _mockIunitOfWork.Setup(u => u.Categories).Returns(_mockCategoryRepository.Object);
            _useCase = new ReadByIdCategoryUseCase(_mockIunitOfWork.Object);
        }
        [Fact]
        public async Task ExecuteAsync_CategoryExists_ReturnsCategory() 
        {
            var categoryId = 1;
            var expectedCategory = new Category { Id = categoryId, Name = "Name Category", Description = "Description Category" };
            _mockCategoryRepository.Setup(r => r.GetByIdAsync(categoryId))
                .Returns(Task.FromResult(expectedCategory));

            //Act
            var result = await _useCase.ExecuteAsync(categoryId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result);
        }
        [Fact]
        public async Task ExecuteAsync_CategoryDoesNotExist_ReturnsNotFound()
        {
            var categoryId = 1;
            _mockCategoryRepository.Setup(r => r.GetByIdAsync(categoryId))
                .Returns(Task.FromResult((Category)null));

            //Act
            var result = await _useCase.ExecuteAsync(categoryId);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("NotFound", result.ErrorCode);
            Assert.Equal("La operación de consulta falló porque la categoría no existe.", result.Message);
        }
        [Fact]
        public async Task WithPersistenceException_ReturnsFailure()
        {
            var categoryId = 1;
            _mockCategoryRepository.Setup(r => r.GetByIdAsync(categoryId))
                .ThrowsAsync(new PersistenceException("Database error"));
            //Act
            var result = await _useCase.ExecuteAsync(categoryId);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("La operación de consulta falló debido a un problema de almacenamiento de datos.", result.Message);
        }
    }
}
