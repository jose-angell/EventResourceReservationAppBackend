using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Locations;
using EventResourceReservationApp.Application.DTOs.Reservations;
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
    public class CreateReservationUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<CreateReservationUseCase>> _logger;
        private readonly Mock<IReservationRepository> _reservationRepository;
        private readonly CreateReservationUseCase _useCase;
        public CreateReservationUseCaseTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _reservationRepository = new Mock<IReservationRepository>();
            _logger = new Mock<ILogger<CreateReservationUseCase>>();
            _unitOfWork.Setup(u => u.Reservations).Returns(_reservationRepository.Object);
            _useCase = new CreateReservationUseCase(_unitOfWork.Object, _logger.Object);
        }

        [Fact]
        public async Task ExecuteAsync_WithValidRequest_RetunrsSuccess()
        {
            // Arrange
            var request = new CreateReservationRequest
            {
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddHours(2),
                TotalAmount = 150.00m,
                ClientComment = "Looking forward to it!",
                ClientPhoneNumber = "123-456-7890",
                LocationId = 1,
                ClientId = Guid.NewGuid()
            };
            _reservationRepository.Setup(r => r.AddAsync(It.IsAny<Reservation>()))
                .Returns(Task.CompletedTask);

            _unitOfWork.Setup(u => u.SaveAsync())
                .Returns(Task.CompletedTask);
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);

            _reservationRepository.Verify(r => r.AddAsync(It.IsAny<Reservation>()), Times.Once);
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
        public async Task ExecuteAsync_WithInvalidRequest_ReturnsFailure()
        {
            // Arrange
            var request = new CreateReservationRequest
            {
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddHours(-2), // Invalid end time
                TotalAmount = -50.00m, // Invalid total amount
                ClientComment = "Looking forward to it!",
                ClientPhoneNumber = "123-456-7890",
                LocationId = 1,
                ClientId = Guid.NewGuid()
            };
            // Act 
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("BadRequest", result.ErrorCode);
            Assert.Equal("StartTime must be earlier than EndTime. (Parameter 'startTime')", result.Message);
            _reservationRepository.Verify(r => r.AddAsync(It.IsAny<Reservation>()), Times.Never);
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
        public async Task ExecuteAsync_WhenPersistenceFails_ReturnsFailure()
        {
            // Arrange
            var request = new CreateReservationRequest
            {
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddHours(2), 
                TotalAmount = 50.00m, 
                ClientComment = "Looking forward to it!",
                ClientPhoneNumber = "123-456-7890",
                LocationId = 1,
                ClientId = Guid.NewGuid()
            };
            _reservationRepository.Setup(r => r.AddAsync(It.IsAny<Reservation>()))
                .ThrowsAsync(new PersistenceException("Database error"));
            // Act 
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("PersistenceError", result.ErrorCode);
            Assert.Equal("La operación de creación falló debido a un problema de almacenamiento de datos.", result.Message);
            _unitOfWork.Verify(u => u.SaveAsync(), Times.Never);
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
