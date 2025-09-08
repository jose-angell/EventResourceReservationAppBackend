using Castle.Core.Logging;
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
    public class UpdateReservationCarItemUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILogger<UpdateReservationCarItemUseCase>> _mockLogger;
        private readonly Mock<IReservationCarItemRepository> _mockReservationCarItemRepository;
        private readonly UpdateReservationCarItemUseCase _useCase;
        public UpdateReservationCarItemUseCaseTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<UpdateReservationCarItemUseCase>>();
            _mockReservationCarItemRepository = new Mock<IReservationCarItemRepository>();
            _mockUnitOfWork.Setup(u => u.ReservationCarItems).Returns(_mockReservationCarItemRepository.Object);
            _useCase = new UpdateReservationCarItemUseCase(_mockUnitOfWork.Object, _mockLogger.Object);
        }
        [Fact]
        public async Task ExecuteAsync_WithValidRequest_RetunrsSuccess()
        {
            // Arrange
            var request = new UpdateReservationCarItemRequest
            {
                Id = Guid.NewGuid(),
                Quantity = 5
            };
            var existingItem = new ReservationCarItem
            {
                Id = request.Id,
                ClientId = Guid.NewGuid(),
                ResourceId = Guid.NewGuid(),
                Quantity = 2
            };
            _mockReservationCarItemRepository.Setup(r => r.GetByIdAsync(request.Id))
                .ReturnsAsync(existingItem);
            _mockReservationCarItemRepository.Setup(r => r.UpdateAsync(It.IsAny<ReservationCarItem>()))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .Returns(Task.CompletedTask);
            // Act
            var result = await _useCase.ExecuteAsync(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Elemento actualizado exitosamente.", result.Message);

            _mockReservationCarItemRepository.Verify(r => r.UpdateAsync(It.IsAny<ReservationCarItem>()), Times.Once); 
            _mockUnitOfWork.Verify(r => r.SaveAsync(), Times.Once); 
        }
        [Fact]
        public async Task ExecuteAsync_ItemNotFound_ReturnsNotFound()
        {
            // Arrange
            var request = new UpdateReservationCarItemRequest
            {
                Id = Guid.NewGuid(),
                Quantity = 5
            };
            _mockReservationCarItemRepository.Setup(r => r.GetByIdAsync(request.Id))
                .ReturnsAsync((ReservationCarItem)null);
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("NotFound", result.ErrorCode);
            Assert.Equal("La operación de actualización falló porque el elemento no existe.", result.Message);
        }
        [Fact]
        public async Task ExecuteAsync_WithInvalidQuantity_ReturnsInvalidInput()
        {
            // Arrange
            var request = new UpdateReservationCarItemRequest
            {
                Id = Guid.NewGuid(),
                Quantity = 0
            };
            var existingItem = new ReservationCarItem
            {
                Id = request.Id,
                ClientId = Guid.NewGuid(),
                ResourceId = Guid.NewGuid(),
                Quantity = 2
            };
            _mockReservationCarItemRepository.Setup(r => r.GetByIdAsync(request.Id))
                .ReturnsAsync(existingItem);
            // Act 
            var result = await _useCase.ExecuteAsync(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("InvalidInput", result.ErrorCode);
            Assert.Equal("Quantity must be greater than zero. (Parameter 'quantity')", result.Message);
            _mockReservationCarItemRepository.Verify(u => u.UpdateAsync(It.IsAny<ReservationCarItem>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
        }
        [Fact]
        public async Task ExecuteAsync_WithPersistenceError_ReturnsFailure()
        {
            // Arrange
            var request = new UpdateReservationCarItemRequest
            {
                Id = Guid.NewGuid(),
                Quantity = 2
            };
            var existingItem = new ReservationCarItem
            {
                Id = request.Id,
                ClientId = Guid.NewGuid(),
                ResourceId = Guid.NewGuid(),
                Quantity = 2
            };
            _mockReservationCarItemRepository.Setup(r => r.GetByIdAsync(request.Id))
                .ReturnsAsync(existingItem);
            _mockReservationCarItemRepository.Setup(r => r.UpdateAsync(It.IsAny<ReservationCarItem>()))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .ThrowsAsync(new PersistenceException("Simulated database error."));

            // Act
            var result = await _useCase.ExecuteAsync(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("PersistenceError", result.ErrorCode);
            Assert.Equal("La operación de actualización falló debido a un problema de almacenamiento de datos.", result.Message);
        }
    }
}
