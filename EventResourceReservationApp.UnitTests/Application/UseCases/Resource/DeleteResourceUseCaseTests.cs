using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Application.UseCases.Resources;
using EventResourceReservationApp.Domain;
using EventResourceReservationApp.Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;

namespace EventResourceReservationApp.UnitTests.Application.UseCases.Resource
{
    public class DeleteResourceUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IResourceRepository> _repository;
        private readonly Mock<ILogger<DeleteResourceUseCase>> _logger;
        private readonly DeleteResourceUseCase _useCase;
        public DeleteResourceUseCaseTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<IResourceRepository>();
            _logger = new Mock<ILogger<DeleteResourceUseCase>>();
            _unitOfWork.Setup(u => u.Resources).Returns(_repository.Object);
            _useCase = new DeleteResourceUseCase(_unitOfWork.Object, _logger.Object);
        }
        [Fact]
        public async Task ExecuteAsync_WithValidRequest_ReturnsSuccess()
        {
            // Arrange
            Guid resourceId = Guid.NewGuid();
            var existingResource = new EventResourceReservationApp.Domain.Resource(1, "Projector", "HD Projector", 50, 150.89m, ResourceAuthorizationType.Automatico, 1, Guid.NewGuid());
            _repository.Setup(r => r.GetByIdAsync(resourceId))
                .ReturnsAsync(existingResource);
            _repository.Setup(r => r.RemoveAsync(existingResource))
                .Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _useCase.ExecuteAsync(resourceId);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Recurso eliminado exitosamente.", result.Message);

            _repository.Verify(r => r.RemoveAsync(existingResource), Times.Once);
            _unitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            _logger.Verify(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Never);
        }
        [Fact]
        public async Task ExecuteAsync_ResourceNotFound_ReturnsFailure()
        {
            //Arrange
            Guid resourceId = Guid.NewGuid();
            _repository.Setup(r => r.GetByIdAsync(resourceId))
                .ReturnsAsync((EventResourceReservationApp.Domain.Resource?)null);
            //Act
            var result = await _useCase.ExecuteAsync(resourceId);

            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("NotFound", result.ErrorCode);
            Assert.Equal($"La operación de eliminación falló debido a que no se encontró el recurso con Id '{resourceId}'.", result.Message);
            _logger.Verify(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }
        [Fact]
        public async Task ExecuteAsync_WithPersistenceError_ReturnsFailure()
        {
            // Arrange
            Guid resourceId = Guid.NewGuid();
            var existingResource = new EventResourceReservationApp.Domain.Resource(1, "Projector", "HD Projector", 50, 150.89m, ResourceAuthorizationType.Automatico, 1, Guid.NewGuid());

            _repository.Setup(r => r.GetByIdAsync(resourceId))
                .ReturnsAsync(existingResource);
            _repository.Setup(r => r.RemoveAsync(existingResource))
                .ThrowsAsync(new PersistenceException("Database error"));
            // Act
            var result = await _useCase.ExecuteAsync(resourceId);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("PersistenceError", result.ErrorCode);
            Assert.Equal($"La operación de eliminación falló debido a un problema de almacenamiento de datos para el recurso con Id '{resourceId}'.", result.Message);
            _logger.Verify(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }
    }
}
