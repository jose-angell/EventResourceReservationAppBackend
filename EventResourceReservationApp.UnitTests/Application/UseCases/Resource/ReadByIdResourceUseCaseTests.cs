using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Application.UseCases.Resources;
using EventResourceReservationApp.Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.UnitTests.Application.UseCases.Resource
{
    public class ReadByIdResourceUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IResourceRepository> _repository;
        private readonly Mock<ILogger<ReadByIdResourceUseCase>> _logger;
        private readonly ReadByIdResourceUseCase _useCase;
        public ReadByIdResourceUseCaseTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<IResourceRepository>();
            _logger = new Mock<ILogger<ReadByIdResourceUseCase>>();
            _unitOfWork.Setup(u => u.Resources).Returns(_repository.Object);
            _useCase = new ReadByIdResourceUseCase(_unitOfWork.Object, _logger.Object);
        }
        [Fact]
        public async Task ExecuteAsync_WithValidRequest_ReturnsSuccess()
        {
            // Arrange
            Guid resourceId = Guid.NewGuid();
            var existingResource = new EventResourceReservationApp.Domain.Resource(1, "Projector", "HD Projector", 50, 150.89m, ResourceAuthorizationType.Automatico, 1, Guid.NewGuid());
            _repository.Setup(r => r.GetByIdAsync(resourceId))
                .ReturnsAsync(existingResource);
            // Act
            var result = await _useCase.ExecuteAsync(resourceId);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Recurso encontrado exitosamente.", result.Message);
        }
        [Fact]
        public async Task ExecuteAsync_WithNotFount_ReturnsFailure()
        {
            // Arrange
            Guid resourceId = Guid.NewGuid();
            var existingResource = new EventResourceReservationApp.Domain.Resource(1, "Projector", "HD Projector", 50, 150.89m, ResourceAuthorizationType.Automatico, 1, Guid.NewGuid());
            _repository.Setup(r => r.GetByIdAsync(resourceId))
                .ReturnsAsync((EventResourceReservationApp.Domain.Resource?)null);
            // Act
            var result = await _useCase.ExecuteAsync(resourceId);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal($"La operación de busqueda falló debido a que no se encontró el recurso con Id '{resourceId}'.", result.Message);
        }
        [Fact]
        public async Task ExecuteAsync_WithPersistenceError_ReturnsFailure()
        {
            // Arrange
            Guid resourceId = Guid.NewGuid();
            var existingResource = new EventResourceReservationApp.Domain.Resource(1, "Projector", "HD Projector", 50, 150.89m, ResourceAuthorizationType.Automatico, 1, Guid.NewGuid());
            _repository.Setup(r => r.GetByIdAsync(resourceId))
                .ThrowsAsync(new PersistenceException("Database error"));
            // Act
            var result = await _useCase.ExecuteAsync(resourceId);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal($"La operación de lectura falló debido a un problema de almacenamiento de datos para el recurso con Id '{resourceId}'.", result.Message);
        }
    }
}
