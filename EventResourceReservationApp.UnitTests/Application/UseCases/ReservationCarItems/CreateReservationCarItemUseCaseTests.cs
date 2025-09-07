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
    public class CreateReservationCarItemUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILogger<CreateReservationCarItemUseCase>> _mockLogger;
        private readonly Mock<IReservationCarItemRepository> _mockReservationCarItemRepository;
        private readonly CreateReservationCarItemUseCase _useCase;
        public CreateReservationCarItemUseCaseTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<CreateReservationCarItemUseCase>>();
            _mockReservationCarItemRepository = new Mock<IReservationCarItemRepository>();
            _mockUnitOfWork.Setup(u => u.ReservationCarItems).Returns(_mockReservationCarItemRepository.Object);
            _useCase = new CreateReservationCarItemUseCase(_mockUnitOfWork.Object, _mockLogger.Object);
        }
        [Fact]
        public async Task ExecuteAsync_WithValidRequest_RetunrsSuccess()
        {
            // Assert
            var request = new CreateReservationCarItemRequest
            {
                ClientId = Guid.NewGuid(),
                ResourceId = Guid.NewGuid(),
                Quantity = 2
            };
            _mockReservationCarItemRepository.Setup(r => r.GetByClientIdAndResourceIdAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((ReservationCarItem)null);
            _mockReservationCarItemRepository.Setup(r => r.AddAsync(It.IsAny<ReservationCarItem>()))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _useCase.ExecuteAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

            _mockReservationCarItemRepository.Verify(r => r.AddAsync(It.IsAny<ReservationCarItem>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }
        [Fact]
        public async Task ExecuteAsync_WithInvalidRequestItem_ReturnsFailure()
        {
            // Assert
            var request = new CreateReservationCarItemRequest
            {
                ClientId = Guid.NewGuid(),
                ResourceId = Guid.NewGuid(),
                Quantity = 2
            };
            var existingItem = new ReservationCarItem(Guid.NewGuid(), Guid.NewGuid(), 1);
            existingItem.Id = Guid.NewGuid();
            _mockReservationCarItemRepository.Setup(r => r.GetByClientIdAndResourceIdAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync(existingItem);
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Conflict", result.ErrorCode);
            Assert.Equal($"La operación de Creacion falló debido a una duplicación de recurso para un carrito de reservas '{request.ResourceId}'.", result.Message);

            // Verificamos que los métodos AddAsync y SaveAsync nunca fueron llamados.
            _mockReservationCarItemRepository.Verify(r => r.AddAsync(It.IsAny<ReservationCarItem>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
        }
        [Fact]
        public async Task ExecuteAsync_WithInvalidRequest_ReturnsFailure()
        {
            // Assert
            var request = new CreateReservationCarItemRequest
            {
                ClientId = Guid.Empty,
                ResourceId = Guid.NewGuid(),
                Quantity = 2
            };
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("InvalidInput", result.ErrorCode);
            Assert.Equal("ClientId cannot be empty. (Parameter 'clientId')", result.Message);

            // Verificamos que los métodos de persistencia nunca fueron llamados.
            _mockUnitOfWork.Verify(u => u.ReservationCarItems.AddAsync(It.IsAny<ReservationCarItem>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
        }
        [Fact]
        public async Task ExecuteAsync_WithPersistenceError_ReturnsFailure()
        {
            // Assert
            var request = new CreateReservationCarItemRequest
            {
                ClientId = Guid.NewGuid(),
                ResourceId = Guid.NewGuid(),
                Quantity = 2
            };
            _mockReservationCarItemRepository.Setup(r => r.GetByClientIdAndResourceIdAsync(
                It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((ReservationCarItem)null);
            _mockReservationCarItemRepository.Setup(r => r.AddAsync(It.IsAny<ReservationCarItem>()))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .ThrowsAsync(new PersistenceException("Database error"));
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("PersistenceError", result.ErrorCode);
            Assert.Equal("Ocurrió un error al intentar guardar el elemento. Por favor, inténtelo de nuevo más tarde.", result.Message);
            _mockReservationCarItemRepository.Verify(r => r.AddAsync(It.IsAny<ReservationCarItem>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }
    }
}
