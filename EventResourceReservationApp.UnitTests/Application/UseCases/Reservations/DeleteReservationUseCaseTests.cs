using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Application.UseCases.Reservations;
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
    public class DeleteReservationUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IReservationRepository> _reservationRepository;
        private readonly Mock<ILogger<DeleteReservationUseCase>> _logger;
        private readonly DeleteReservationUseCase _useCase;
        public DeleteReservationUseCaseTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _reservationRepository = new Mock<IReservationRepository>();
            _logger = new Mock<ILogger<DeleteReservationUseCase>>();
            _unitOfWork.Setup(u => u.Reservations).Returns(_reservationRepository.Object);
            _useCase = new DeleteReservationUseCase(_unitOfWork.Object, _logger.Object);
        }
        [Fact]
        public async Task ExecuteAsync_WithValidRequest_ReturnsSuccess()
        {
            // Arrange
            Guid reservationId = Guid.NewGuid();
            var existingReservation = new Reservation(DateTime.Now, DateTime.Now.AddHours(2), 12m, "Looking forward to it!", "123-456-7890", 1, Guid.NewGuid());
            _reservationRepository.Setup(r => r.GetByIdAsync(reservationId))
                .ReturnsAsync(existingReservation);
            _reservationRepository.Setup(r => r.RemoveAsync(existingReservation))
                .Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _useCase.ExecuteAsync(reservationId);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Reserva eliminada exitosamente.", result.Message);

            _reservationRepository.Verify(r => r.RemoveAsync(existingReservation), Times.Once);
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
            _reservationRepository.Setup(r => r.GetByIdAsync(reservationId))
                .ReturnsAsync((Reservation?)null);
            //Act
            var result = await _useCase.ExecuteAsync(reservationId);

            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("NotFound", result.ErrorCode);
            Assert.Equal($"La operación de eliminación falló debido a que no se encontró la reserva con Id '{reservationId}'.", result.Message);
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
            var existingReservation = new Reservation(DateTime.Now, DateTime.Now.AddHours(2), 12m, "Looking forward to it!", "123-456-7890", 1, Guid.NewGuid());
            _reservationRepository.Setup(r => r.GetByIdAsync(reservationId))
                .ReturnsAsync(existingReservation);
            _reservationRepository.Setup(r => r.RemoveAsync(existingReservation))
                .ThrowsAsync(new PersistenceException("Database error"));
            // Act
            var result = await _useCase.ExecuteAsync(reservationId);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("PersistenceError", result.ErrorCode);
            Assert.Equal("La operación de eliminación falló debido a un problema de almacenamiento de datos.", result.Message);
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
