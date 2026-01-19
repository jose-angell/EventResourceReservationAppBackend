using Castle.Core.Logging;
using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Reservations;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Application.UseCases.Reservations;
using EventResourceReservationApp.Domain.Enums;
using EventResourceReservationApp.Domain;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.UnitTests.Application.UseCases.Reservations
{
    public class UpdateStatusReservationUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IReservationRepository> _repository;
        private readonly UpdateStatusReservationUseCase _useCase;
        private readonly Mock<ILogger<UpdateStatusReservationUseCase>> _logger;
        public UpdateStatusReservationUseCaseTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<IReservationRepository>();
            _logger = new Mock<ILogger<UpdateStatusReservationUseCase>>();
            _unitOfWork.Setup(u => u.Reservations).Returns(_repository.Object);
            _useCase = new UpdateStatusReservationUseCase(_unitOfWork.Object, _logger.Object);
        }
        [Fact]
        public async Task ExecuteAsync_WithValidRequest_ReturnsSuccess()
        {
            //Arrange
            Guid reservationId = Guid.NewGuid();
            var existingReservation = new Reservation(DateTime.Now, DateTime.Now.AddHours(2), 12m, "Looking forward to it!", "123-456-7890", 1, Guid.NewGuid());
            var updatedReservation = new UpdateStatusReservationRequest
            {
                Id = reservationId,
                AdminId = Guid.NewGuid(),
                AdminComment = "Updated comment",
                StatusId = ReservationStatus.Confirmed
            };
            _repository.Setup(r => r.GetByIdAsync(reservationId))
                .ReturnsAsync(existingReservation);

            _repository.Setup(r => r.UpdateAsync(It.IsAny<Reservation>()))
               .Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _useCase.ExecuteAsync(updatedReservation);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Reserva editada exitosamente.", result.Message);

            _repository.Verify(r => r.UpdateAsync(existingReservation), Times.Once);
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
        public async Task ExecuteAsync_ReservationNotFound_ReturnsFailure()
        {
            //Arrange
            Guid reservationId = Guid.NewGuid();
            var updatedReservation = new UpdateStatusReservationRequest
            {
                Id = reservationId,
                AdminId = Guid.NewGuid(),
                AdminComment = "Updated comment",
                StatusId = ReservationStatus.Confirmed
            };
            _repository.Setup(r => r.GetByIdAsync(reservationId))
                .ReturnsAsync((Reservation?)null);
            //Act
            var result = await _useCase.ExecuteAsync(updatedReservation);

            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("NotFound", result.ErrorCode);
            Assert.Equal($"La operación de edición falló debido a que no se encontró la reserva con Id '{reservationId}'.", result.Message);
            _logger.Verify(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }
        [Fact]
        public async Task ExecuteAsync_WithInvalidRequest_ReturnsFailure()
        {
            //Arrange
            Guid reservationId = Guid.NewGuid();
            var existingReservation = new Reservation(DateTime.Now, DateTime.Now.AddHours(2), 12m, "Looking forward to it!", "123-456-7890", 1, Guid.NewGuid());
            var updatedReservation = new UpdateStatusReservationRequest
            {
                Id = reservationId,
                AdminId = Guid.Empty, // Invalid AdminId
                AdminComment = "Updated comment",
                StatusId = ReservationStatus.Confirmed
            };
            _repository.Setup(r => r.GetByIdAsync(reservationId))
                .ReturnsAsync(existingReservation);
            // Act 
            var result = await _useCase.ExecuteAsync(updatedReservation);
            // Assert 
            Assert.False(result.IsSuccess);
            Assert.Equal("InvalidInput", result.ErrorCode);
            Assert.Equal("AdminId cannot be empty. (Parameter 'adminId')", result.Message);
            _repository.Verify(r => r.UpdateAsync(It.IsAny<Reservation>()), Times.Never);
            _unitOfWork.Verify(u => u.SaveAsync(), Times.Never);
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
            Guid reservationId = Guid.NewGuid();
            var updatedReservation = new UpdateStatusReservationRequest
            {
                Id = reservationId,
                AdminId = Guid.NewGuid(),
                AdminComment = "Updated comment",
                StatusId = ReservationStatus.Confirmed
            };
            var existingReservation = new Reservation(DateTime.Now, DateTime.Now.AddHours(2), 12m, "Looking forward to it!", "123-456-7890", 1, Guid.NewGuid());
            _repository.Setup(r => r.GetByIdAsync(reservationId))
                .ReturnsAsync(existingReservation);
            _repository.Setup(r => r.UpdateAsync(It.IsAny<Reservation>()))
               .ThrowsAsync(new PersistenceException("Database error"));
            // Act
            var result = await _useCase.ExecuteAsync(updatedReservation);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("PersistenceError", result.ErrorCode);
            Assert.Equal("La operación de edición falló debido a un problema de almacenamiento de datos.", result.Message);
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
