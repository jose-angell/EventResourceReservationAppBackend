using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.ReservationCarItems;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Application.UseCases.ReservationCarItems;
using EventResourceReservationApp.Domain;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.UnitTests.Application.UseCases.ReservationCarItems
{
    public class DeleteReservationCarItemUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILogger<DeleteReservationCarItemUseCase>> _mockLogger;
        private readonly Mock<IReservationCarItemRepository> _mockReservationCarItemRepository;
        private readonly DeleteReservationCarItemUseCase _useCase;
        public DeleteReservationCarItemUseCaseTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<DeleteReservationCarItemUseCase>>();
            _mockReservationCarItemRepository = new Mock<IReservationCarItemRepository>();
            _mockUnitOfWork.Setup(u => u.ReservationCarItems).Returns(_mockReservationCarItemRepository.Object);
            _useCase = new DeleteReservationCarItemUseCase(_mockUnitOfWork.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task ExecuteAsync_WithValidRequest_ReturnsSucces()
        {
            // Arrange
            var Id = Guid.NewGuid();

            var existingItem = new ReservationCarItem
            {
                Id = Id,
                ClientId = Guid.NewGuid(),
                ResourceId = Guid.NewGuid(),
                Quantity = 2
            };
            _mockReservationCarItemRepository.Setup(r => r.GetByIdAsync(Id))
                .ReturnsAsync(existingItem);
            _mockReservationCarItemRepository.Setup(r => r.RemoveAsync(It.IsAny<ReservationCarItem>()))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .Returns(Task.CompletedTask);
            // Act
            var result = await _useCase.ExecuteAsync(Id);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Elemento eliminado exitosamente.", result.Message);
            _mockReservationCarItemRepository.Verify(r => r.RemoveAsync(It.IsAny<ReservationCarItem>()), Times.Once);
            _mockUnitOfWork.Verify(r => r.SaveAsync(), Times.Once);
            _mockLogger.Verify(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Never);
        }
        [Fact]
        public async Task ExecuteAsync_ItemNotFound_ReturnsNotFound()
        {
            // Arrange
            var Id = Guid.NewGuid();
            _mockReservationCarItemRepository.Setup(r => r.GetByIdAsync(Id))
                .ReturnsAsync((ReservationCarItem)null);

            // Act
            var result = await _useCase.ExecuteAsync(Id);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("NotFound", result.ErrorCode);
            Assert.Equal("La operación de eliminación falló porque el elemento no existe.", result.Message);

            _mockReservationCarItemRepository.Verify(r => r.RemoveAsync(It.IsAny<ReservationCarItem>()), Times.Never);
            _mockUnitOfWork.Verify(r => r.SaveAsync(), Times.Never);
            _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains($"Fallo al eliminar el elemento de Carrito de reservas: No se encontró el elemento con Id '{Id}'.")),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
        }
        [Fact]
        public async Task ExecuteAsync_WithPersistenceError_ReturnsFailure()
        {
            var Id = Guid.NewGuid();
            var existingItem = new ReservationCarItem
            {
                Id = Id,
                ClientId = Guid.NewGuid(),
                ResourceId = Guid.NewGuid(),
                Quantity = 2
            };
            _mockReservationCarItemRepository.Setup(r => r.GetByIdAsync(Id))
                .ReturnsAsync(existingItem);
            _mockReservationCarItemRepository.Setup(r => r.RemoveAsync(It.IsAny<ReservationCarItem>()))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .ThrowsAsync(new PersistenceException("Database error"));
            // Act
            var result = await _useCase.ExecuteAsync(Id);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("PersistenceError", result.ErrorCode);
            Assert.Equal("La operación de eliminación falló debido a un problema de almacenamiento de datos.", result.Message);
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("error de persistencia")),
                    It.IsAny<PersistenceException>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }
    }
}
