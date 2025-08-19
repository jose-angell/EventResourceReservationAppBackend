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
    public class CreateCategoryUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly CreateCategoryUseCase _useCase;
        private readonly Mock<ILogger<CreateCategoryUseCase>> _mockLogger;
        public CreateCategoryUseCaseTests()
        {
            // 2. Inicializamos ambos mocks.
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockLogger = new Mock<ILogger<CreateCategoryUseCase>>();

            // 3. ¡La clave está aquí! Le decimos al _mockUnitOfWork que,
            //    cuando se acceda a su propiedad 'Categories', devuelva el objeto
            //    mockeado de _mockCategoryRepository.
            _mockUnitOfWork.Setup(u => u.Categories).Returns(_mockCategoryRepository.Object);

            // 4. Creamos la instancia del caso de uso, pasándole el objeto mockeado de UnitOfWork.
            _useCase = new CreateCategoryUseCase(_mockUnitOfWork.Object,_mockLogger.Object);
        }
        [Fact]
        public async Task ExecuteAsync_WithValidRequest_RetunrsSuccess()
        {
            // Arrange
            var request = new CreateCategoryRequest
            {
                Name = "Test Category",
                Description = "Description for test category",
                CreatedByUserId = Guid.NewGuid()
            };

            // 4. Configuramos el mock del REPOSITORIO (no del UnitOfWork)
            //    para que GetFirstOrDefaultAsync devuelva null.
            _mockCategoryRepository.Setup(r => r.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Category, bool>>>(), // Esto es clave
                It.IsAny<string>())) // Si tu método tiene un segundo parámetro para 'includeProperties', también debes mockearlo.
                .ReturnsAsync((Category)null);

            // 5. Configuramos los otros métodos del repositorio y el Unit of Work.
            _mockCategoryRepository.Setup(r => r.AddAsync(It.IsAny<Category>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _useCase.ExecuteAsync(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal("Test Category", result.Data.Name);

            // 6. Verificamos las llamadas en los MOCKS correctos.
            _mockCategoryRepository.Verify(r => r.AddAsync(It.IsAny<Category>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        
        }
        [Fact]
        public async Task ExecuteAsync_WithExistingCategoryName_ReturnsFailure()
        {
            // Arrange
            var request = new CreateCategoryRequest
            {
                Name = "Existing Category",
                Description = "Description",
                CreatedByUserId = Guid.NewGuid()
            };
            var existingCategory = new Category(request.Name, "Old Description", Guid.NewGuid());
            existingCategory.Id = 1;
            _mockCategoryRepository.Setup(r => r.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Category, bool>>>(), // Esto es clave
                It.IsAny<string>())) // Si tu método tiene un segundo parámetro para 'includeProperties', también debes mockearlo.
                .ReturnsAsync(existingCategory);
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Conflict", result.ErrorCode);
            Assert.Equal($"La operación de actualización falló debido a una duplicación de nombre '{request.Name}'.", result.Message);

            // Verificamos que los métodos AddAsync y SaveAsync nunca fueron llamados.
            _mockUnitOfWork.Verify(u => u.Categories.AddAsync(It.IsAny<Category>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
        }
        [Fact]
        public async Task ExecuteAsync_WithInvalidRequest_ReturnsFailure()
        {
            // Arrange
            var request = new CreateCategoryRequest
            {
                Name = "", // Nombre inválido que lanzará ArgumentException en la entidad.
                Description = "Description",
                CreatedByUserId = Guid.NewGuid()
            };

            // El constructor de la entidad lanzará la excepción, por lo que no es necesario simular el repositorio.

            // Act
            var result = await _useCase.ExecuteAsync(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("InvalidInput", result.ErrorCode);
            Assert.Equal("Name cannot be null or empty. (Parameter 'name')", result.Message);

            // Verificamos que los métodos de persistencia nunca fueron llamados.
            _mockUnitOfWork.Verify(u => u.Categories.AddAsync(It.IsAny<Category>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
        }
        [Fact]
        public async Task ExecuteAsync_WithPersistenceError_ReturnsFailure()
        {
            // Arrange
            var request = new CreateCategoryRequest
            {
                Name = "Test Category",
                Description = "Description",
                CreatedByUserId = Guid.NewGuid()
            };

            // Simulamos que el repositorio no encuentra la categoría existente.
            _mockCategoryRepository.Setup(r => r.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Category, bool>>>(), // Esto es clave
                It.IsAny<string>())) // Si tu método tiene un segundo parámetro para 'includeProperties', también debes mockearlo.
                .ReturnsAsync((Category)null);

            // Configuramos el mock de SaveAsync para que lance una excepción de persistencia.
            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .ThrowsAsync(new PersistenceException("Simulated database error."));

            // Act
            var result = await _useCase.ExecuteAsync(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("La operación de creación falló debido a un problema de almacenamiento de datos.", result.Message);

            // Verificamos que AddAsync fue llamado y SaveAsync también.
            _mockUnitOfWork.Verify(u => u.Categories.AddAsync(It.IsAny<Category>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }
    }
}
