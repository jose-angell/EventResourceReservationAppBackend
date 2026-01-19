using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Reservations;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Application.UseCases.Reservations;
using EventResourceReservationApp.Domain;
using EventResourceReservationApp.Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.UnitTests.Application.UseCases.Reservations
{
    public class ReadByIdReservationUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IReservationRepository> _repository;
        private readonly Mock<ILogger<ReadByIdReservationUseCase>> _logger;
        private readonly ReadByIdReservationUseCase _useCase;
        public ReadByIdReservationUseCaseTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<IReservationRepository>();
            _logger = new Mock<ILogger<ReadByIdReservationUseCase>>();
            _unitOfWork.Setup(u => u.Reservations).Returns(_repository.Object);
            _useCase = new ReadByIdReservationUseCase(_unitOfWork.Object, _logger.Object);
        }
        [Fact]
        public async Task ExecuteAsync_WithValidRequest_ReturnsLocation()
        {
            // Arrange
            Guid reservationId = Guid.NewGuid();
            var existingReservation = new Reservation(DateTime.Now, DateTime.Now.AddHours(2), 12m, "Looking forward to it!", "123-456-7890", 1, Guid.NewGuid());
            _repository.Setup(r => r.GetByIdAsync(reservationId))
                .ReturnsAsync(existingReservation);
            // Act
            var result = await _useCase.ExecuteAsync(reservationId);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            _repository.Verify(r => r.GetByIdAsync(reservationId), Times.Once);
        }
        [Fact]
        public async Task ExecuteAsync_ReservationNotFound_ReturnsFailure()
        {
            //Arrange
            Guid reservationId = Guid.NewGuid();
            
            _repository.Setup(r => r.GetByIdAsync(reservationId))
                .ReturnsAsync((Reservation?)null);
            //Act
            var result = await _useCase.ExecuteAsync(reservationId);

            //Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("NotFound", result.ErrorCode);
            Assert.Equal($"La operación de consulta falló debido a que no se encontró la reserva con Id '{reservationId}'.", result.Message);
            _logger.Verify(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_WhenPersistenceErrorOccurs_ReturnsPersistenceError()
        {
            // Arrange
            Guid reservationId = Guid.NewGuid();
            _repository.Setup(r => r.GetByIdAsync(reservationId))
               .ThrowsAsync(new PersistenceException("Database error"));
            // Act
            var result = await _useCase.ExecuteAsync(reservationId);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("PersistenceError", result.ErrorCode);
            Assert.Equal("La operación de consulta falló debido a un problema de almacenamiento de datos.", result.Message);
        }
    }
}
