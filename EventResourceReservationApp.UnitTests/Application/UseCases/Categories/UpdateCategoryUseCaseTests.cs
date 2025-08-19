using Castle.Core.Logging;
using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Categories;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Application.UseCases.Categories;
using EventResourceReservationApp.Domain;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.UnitTests.Application.UseCases.Categories
{
    public class UpdateCategoryUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<ILogger<UpdateCategoryUseCase>> _mockLogger;
        private readonly UpdateCategoryUseCase _useCase;
        public UpdateCategoryUseCaseTests()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<UpdateCategoryUseCase>>();
            _mockUnitOfWork.Setup(u => u.Categories).Returns(_mockCategoryRepository.Object);
            _useCase = new UpdateCategoryUseCase(_mockUnitOfWork.Object, _mockLogger.Object);
        }
        [Fact]
        public async Task ExecuteAsync_WithValidRequest_ReturnsSuccess()
        {
            // Arrange
            var request = new UpdateCategoryRequest
            {
                Id = 1,
                Name = "Update Category",
                Description = "Updated Description"
            };
            //validar que exista
            _mockCategoryRepository.Setup(r => r.GetByIdAsync(request.Id))
                .ReturnsAsync(new Category { Id = request.Id, Name = "Old Category", Description = "Old Description" });
            //validar que no se repita el nombre
            _mockCategoryRepository.Setup(r => r.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Category, bool>>>(), // Esto es clave
                It.IsAny<string>())) // Si tu método tiene un segundo parámetro para 'includeProperties', también debes mockearlo.
                .ReturnsAsync((Category)null);
            _mockCategoryRepository.Setup(u => u.UpdateAsync(It.IsAny<Category>()))
               .Returns(Task.CompletedTask);
            //Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Categoría actualizada exitosamente.", result.Message);
            _mockCategoryRepository.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }
        [Fact]
        public async Task ExecuteAsync_WithValidRequestAndNewName_ReturnsSuccess()
        {
            // Arrange
            var request = new UpdateCategoryRequest
            {
                Id = 1,
                Name = "New Category Name",
                Description = "Updated Description"
            };
            var existingCategory = new Category { Id = 1, Name = "Old Category Name", Description = "Old Description" };

            _mockCategoryRepository.Setup(r => r.GetByIdAsync(request.Id))
                .ReturnsAsync(existingCategory);

            _mockCategoryRepository.Setup(r => r.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Category, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync((Category)null);

            _mockUnitOfWork.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _useCase.ExecuteAsync(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Categoría actualizada exitosamente.", result.Message);

            _mockCategoryRepository.Verify(r => r.GetByIdAsync(request.Id), Times.Once);
            _mockCategoryRepository.Verify(r => r.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<string>()), Times.Once);
            _mockCategoryRepository.Verify(r => r.UpdateAsync(It.Is<Category>(c => c.Name == request.Name && c.Description == request.Description)), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }
        [Fact]
        public async Task ExecuteAsync_WithSameName_DoesNotCheckForDuplicationAndReturnsSuccess()
        {
            // Arrange
            var request = new UpdateCategoryRequest
            {
                Id = 1,
                Name = "Old Category Name", // El nombre es el mismo que el que se encuentra
                Description = "New Description"
            };
            var existingCategory = new Category { Id = 1, Name = "Old Category Name", Description = "Old Description" };

            _mockCategoryRepository.Setup(r => r.GetByIdAsync(request.Id))
                .ReturnsAsync(existingCategory);

            _mockUnitOfWork.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _useCase.ExecuteAsync(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Categoría actualizada exitosamente.", result.Message);

            _mockCategoryRepository.Verify(r => r.GetByIdAsync(request.Id), Times.Once);
            // Verificamos que GetFirstOrDefaultAsync NO se llamó
            _mockCategoryRepository.Verify(r => r.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<string>()), Times.Never);
            _mockCategoryRepository.Verify(r => r.UpdateAsync(It.Is<Category>(c => c.Name == request.Name && c.Description == request.Description)), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }
        [Fact]
        public async Task ExecuteAsync_CategoryNotFound_ReturnsNotFound()
        {
            // Arrange
            var request = new UpdateCategoryRequest
            {
                Id = 1,
                Description = "Updated Description",
                Name = "Update Category"
            };
            //validar que exista
            _mockCategoryRepository.Setup(r => r.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Category, bool>>>(), // Esto es clave
                It.IsAny<string>())) // Si tu método tiene un segundo parámetro para 'includeProperties', también debes mockearlo.
                .ReturnsAsync((Category)null);

            //Act
            var result = await _useCase.ExecuteAsync(request);

            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("NotFound", result.ErrorCode);
            Assert.Equal("La operación de actualización falló porque la categoría no existe.", result.Message);
            _mockCategoryRepository.Verify(u => u.UpdateAsync(It.IsAny<Category>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
        }
        [Fact]
        public async Task ExecuteAsync_CategoryConflict_ReturnsConflict()
        {
            //Arrange
            var request = new UpdateCategoryRequest
            {
                Id = 1,
                Name = "Update Category",
                Description = "Updated Description"
            };
            // validar que exista
            _mockCategoryRepository.Setup(r => r.GetByIdAsync(request.Id))
                .ReturnsAsync(new Category { Id = request.Id, Name = "Old Category", Description = "Old Description" });
            //validar que no se repita el nombre
            _mockCategoryRepository.Setup(r => r.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Category, bool>>>(), // Esto es clave
                It.IsAny<string>())) // Si tu método tiene un segundo parámetro para 'includeProperties', también debes mockearlo.
                .ReturnsAsync(new Category { Id = 2, Name = "Old Category", Description = "Old Description" });
            //Act
            var result = await _useCase.ExecuteAsync(request);

            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Conflict", result.ErrorCode);
            Assert.Equal($"La operación de actualización falló debido a una duplicación de nombre '{request.Name}'.", result.Message);
            _mockCategoryRepository.Verify(u => u.UpdateAsync(It.IsAny<Category>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
        }
        [Fact]
        public async Task ExecuteAsync_WithInvalidRequest_ReturnsFailure()
        {
            //Arrange
            var request = new UpdateCategoryRequest
            {
                Id = 1,
                Description = "",
                Name = ""
            };
            //validar que exista
            _mockCategoryRepository.Setup(r => r.GetByIdAsync(request.Id))
                .ReturnsAsync(new Category { Id = request.Id, Name = "Old Category", Description = "Old Description" });
            //validar que no se repita el nombre
            _mockCategoryRepository.Setup(r => r.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Category, bool>>>(), // Esto es clave
                It.IsAny<string>())) // Si tu método tiene un segundo parámetro para 'includeProperties', también debes mockearlo.
                .ReturnsAsync((Category)null);
            //Act 
            var result = await _useCase.ExecuteAsync(request);
            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("InvalidInput", result.ErrorCode);
            Assert.Equal("Name cannot be null or empty. (Parameter 'name')", result.Message);

            // Verificamos que los métodos de persistencia nunca fueron llamados.
            _mockCategoryRepository.Verify(u => u.UpdateAsync(It.IsAny<Category>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
        }
        [Fact]
        public async Task ExecuteAsync_WithPersistenceError_ReturnsFailure()
        {
            // Arrange
            var request = new UpdateCategoryRequest
            {
                Id = 1,
                Name = "Update Category",
                Description = "Updated Description"
            };
            _mockCategoryRepository.Setup(r => r.GetByIdAsync(request.Id))
                .ReturnsAsync(new Category { Id = request.Id, Name = "Old Category", Description = "Old Description" });
            //validar que no se repita el nombre
            _mockCategoryRepository.Setup(r => r.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Category, bool>>>(), // Esto es clave
                It.IsAny<string>())) // Si tu método tiene un segundo parámetro para 'includeProperties', también debes mockearlo.
                .ReturnsAsync((Category)null);
            _mockCategoryRepository.Setup(u => u.UpdateAsync(It.IsAny<Category>()))
               .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .ThrowsAsync(new PersistenceException("Simulated database error."));

            // Act
            var result = await _useCase.ExecuteAsync(request);
            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("La operación de actualización falló debido a un problema de almacenamiento de datos.", result.Message);

            // Verificamos que AddAsync fue llamado y SaveAsync también.
            _mockCategoryRepository.Verify(u => u.UpdateAsync(It.IsAny<Category>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }
    }
}
