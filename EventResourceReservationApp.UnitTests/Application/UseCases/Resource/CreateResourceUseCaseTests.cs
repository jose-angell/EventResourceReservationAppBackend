using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Resources;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Application.UseCases.Resources;
using EventResourceReservationApp.Domain;
using EventResourceReservationApp.Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.UnitTests.Application.UseCases.Resource
{
    public class CreateResourceUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IResourceRepository> _repository;
        private readonly Mock<ILogger<CreateResourceUseCase>> _logger;
        private readonly CreateResourceUseCase _useCase;
        public CreateResourceUseCaseTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<IResourceRepository>();
            _logger = new Mock<ILogger<CreateResourceUseCase>>();
            _unitOfWork.Setup(u => u.Resources).Returns(_repository.Object);
            _useCase = new CreateResourceUseCase(_unitOfWork.Object, _logger.Object);
        }
        [Fact]
        public async Task ExecuteAsync_WithValidRequest_ReturnsSuccess()
        {
            // Arrange
            var request = new CreateResourceRequest
            {
                CategoryId = 1,
                StatusId = 1,
                Name = "Projector",
                Description = "HD Projector",
                Quantity = 50,
                Price = 150.89m,
                AuthorizationType = ResourceAuthorizationType.Automatico,
                LocationId = 1,
                CreatedByUserId = Guid.NewGuid()
            };
            _repository.Setup(r => r.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<EventResourceReservationApp.Domain.Resource, bool>>>(), 
                It.IsAny<string>())) 
                .ReturnsAsync((EventResourceReservationApp.Domain.Resource)null);
            _repository.Setup(r => r.AddAsync(It.IsAny<EventResourceReservationApp.Domain.Resource>()))
                .Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Recurso creado exitosamente.", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal(request.Name, result.Data.Name);
            _repository.Verify(r => r.AddAsync(It.IsAny<EventResourceReservationApp.Domain.Resource>()), Times.Once);
            _unitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }
        [Fact]
        public async Task ExecuteAsync_WithExistingResource_ReturnsFailure()
        {
            // Arrange
            var existingResource = new EventResourceReservationApp.Domain.Resource(1, "Projector", "HD Projector", 50, 150.89m, ResourceAuthorizationType.Automatico, 1, Guid.NewGuid());
            var request = new CreateResourceRequest
            {
                CategoryId = 1,
                StatusId = 1,
                Name = "Projector",
                Description = "HD Projector",
                Quantity = 50,
                Price = 150.89m,
                AuthorizationType = ResourceAuthorizationType.Automatico,
                LocationId = 1,
                CreatedByUserId = Guid.NewGuid()
            };
            _repository.Setup(r => r.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<EventResourceReservationApp.Domain.Resource, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(existingResource);
            _repository.Setup(r => r.AddAsync(It.IsAny<EventResourceReservationApp.Domain.Resource>()))
                .Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Conflict", result.ErrorCode);
            Assert.Equal($"La operación de creación falló debido a una duplicación de nombre '{request.Name}' en la ubicación '{request.LocationId}'.", result.Message);
            _repository.Verify(r => r.AddAsync(It.IsAny<EventResourceReservationApp.Domain.Resource>()), Times.Never);
        }
        [Fact]
        public async Task ExecuteAsync_WithInvalidRequest_ReturnsFailure()
        {
            // Arrange
            var request = new CreateResourceRequest
            {
                CategoryId = 1,
                StatusId = 1,
                Name = "",
                Description = "HD Projector",
                Quantity = 50,
                Price = 150.89m,
                AuthorizationType = ResourceAuthorizationType.Automatico,
                LocationId = 1,
                CreatedByUserId = Guid.NewGuid()
            };
            _repository.Setup(r => r.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<EventResourceReservationApp.Domain.Resource, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync((EventResourceReservationApp.Domain.Resource)null);
            // Act
            var result = await _useCase.ExecuteAsync(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("InvalidInput", result.ErrorCode);
            Assert.Equal("Name cannot be null or empty. (Parameter 'name')", result.Message);
        }
        [Fact]
        public async Task ExecuteAsync_WhenPersistenceFails_ReturnsFailure()
        {
            // Arrange
            var request = new CreateResourceRequest
            {
                CategoryId = 1,
                StatusId = 1,
                Name = "Projector",
                Description = "HD Projector",
                Quantity = 50,
                Price = 150.89m,
                AuthorizationType = ResourceAuthorizationType.Automatico,
                LocationId = 1,
                CreatedByUserId = Guid.NewGuid()
            };
            _repository.Setup(r => r.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<EventResourceReservationApp.Domain.Resource, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync((EventResourceReservationApp.Domain.Resource)null);
            _repository.Setup(r => r.AddAsync(It.IsAny<EventResourceReservationApp.Domain.Resource>()))
                .Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.SaveAsync()).ThrowsAsync(new PersistenceException("Simulated database error."));
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("PersistenceError", result.ErrorCode);
            Assert.Equal("La operación de creación falló debido a un problema de almacenamiento de datos.", result.Message);
        }
    }
}
