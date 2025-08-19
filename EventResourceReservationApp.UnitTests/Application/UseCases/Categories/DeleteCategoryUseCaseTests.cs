using Castle.Core.Logging;
using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Application.UseCases.Categories;
using EventResourceReservationApp.Domain;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.UnitTests.Application.UseCases.Categories
{
    public class DeleteCategoryUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<ILogger<DeleteCategoryUseCase>> _mockLogger;
        private readonly DeleteCategoryUseCase _useCase;
        public DeleteCategoryUseCaseTests()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<DeleteCategoryUseCase>>();   
            _mockUnitOfWork.Setup(u => u.Categories).Returns(_mockCategoryRepository.Object);
            _useCase = new DeleteCategoryUseCase(_mockUnitOfWork.Object, _mockLogger.Object);
        }
        [Fact]
        public async Task ExecuteAsync_CategoryExists_ShouldReturnSuccess()
        {
            // Arrange
            var categoriaId = 1;
            _mockCategoryRepository.Setup(r => r.GetByIdAsync(categoriaId))
                .ReturnsAsync(new Category { Id = categoriaId, Name = "Name Category", Description = "Description Category" });
            _mockCategoryRepository.Setup(r => r.RemoveASync(It.IsAny<Category>()));
            _mockUnitOfWork.Setup(u => u.SaveAsync())
                                .Returns(Task.CompletedTask);
            //Act
            var result = await _useCase.ExecuteAsync(categoriaId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Categoría eliminada exitosamente.", result.Message);

            _mockCategoryRepository.Verify(r => r.RemoveASync(It.IsAny<Category>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }
        [Fact]
        public async Task ExecuteAsync_CategoryDoesNotExist_ReturnNotFound()
        {
            // Arrange
            var categoriaId = 1;
            _mockCategoryRepository.Setup(r => r.GetByIdAsync(categoriaId))
                .ReturnsAsync((Category)null);
            // Act
            var result = await _useCase.ExecuteAsync(categoriaId);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("NotFound", result.ErrorCode);
            Assert.Equal("La operación de eliminación falló porque la categoría no existe.", result.Message);
            _mockCategoryRepository.Verify(r => r.RemoveASync(It.IsAny<Category>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
        }
        [Fact]
        public async Task ExecuteAsync_PersistenceException_ReturnsFailure()
        {
            //Arrange
            var categoriaId = 1;
            _mockCategoryRepository.Setup(r => r.GetByIdAsync(categoriaId))
                .ReturnsAsync(new Category { Id = categoriaId, Name = "Name Category", Description = "Description Category" });
            _mockCategoryRepository.Setup(r => r.RemoveASync(It.IsAny<Category>()));
            _mockUnitOfWork.Setup(u => u.SaveAsync())
                               .ThrowsAsync(new PersistenceException("Simulated database error."));
            // Act
            var result = await _useCase.ExecuteAsync(categoriaId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("La operación de eliminación falló debido a un problema de almacenamiento de datos.", result.Message);

            _mockCategoryRepository.Verify(r => r.RemoveASync(It.IsAny<Category>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }
    }
}
