using Castle.Core.Logging;
using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Reviews;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Application.UseCases.Reviews;
using EventResourceReservationApp.Domain;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.UnitTests.Application.UseCases.Reviews
{
    public class UpdateReviewUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILogger<UpdateReviewUseCase>> _logger;
        private readonly Mock<IReviewRepository> _reviewRepository;
        private readonly UpdateReviewUseCase _useCase;
        public UpdateReviewUseCaseTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _logger = new Mock<ILogger<UpdateReviewUseCase>>();
            _reviewRepository = new Mock<IReviewRepository>();
            _mockUnitOfWork.Setup(u => u.Reviews).Returns(_reviewRepository.Object);
            _useCase = new UpdateReviewUseCase(_mockUnitOfWork.Object, _logger.Object);
        }
        [Fact]
        public async Task ExecuteAsync_WithValidRequest_ReturnsSuccess()
        {
            var review = new Review
            {
                Id = Guid.NewGuid(),
                ResourceId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Rating = 4,
                Comment = "Good resource"
            };
            var request = new UpdateReviewRequest
            {
                Id = review.Id,
                Rating = 5,
                Comment = "Excellent resource!"
            };
            _reviewRepository.Setup(r => r.GetById(review.Id))
                .ReturnsAsync(review);
            _reviewRepository.Setup(r => r.UpdateAsync(It.IsAny<Review>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Reseña actualizada exitosamente.", result.Message);

            _reviewRepository.Verify(r => r.UpdateAsync(It.IsAny<Review>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            _logger.Verify(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Never);
        }
        [Fact]
        public async Task ExecuteAsync_ReviewNotFound_ReturnsFailure()
        {
            // Arrange
            var request = new UpdateReviewRequest
            {
                Id = Guid.NewGuid(),
                Rating = 5,
                Comment = "Excellent resource!"
            };
            _reviewRepository.Setup(r => r.GetById(request.Id))
                .ReturnsAsync((Review)null);
            // Act 
            var result = await _useCase.ExecuteAsync(request);
            // Assert 
            Assert.False(result.IsSuccess);
            Assert.Equal("NotFound", result.ErrorCode);
            _reviewRepository.Verify(r => r.UpdateAsync(It.IsAny<Review>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
            _logger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains($"Reseña con Id {request.Id} no encontrada para actualización.")),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
        }
        [Fact]
        public async Task ExecuteAsync_WithInvalidInput_ReturnsFailure()
        {
            // Arrange
            var review = new Review
            {
                Id = Guid.NewGuid(),
                ResourceId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Rating = 4,
                Comment = "Good resource"
            };
            var request = new UpdateReviewRequest
            {
                Id = review.Id,
                Rating = 6, // Invalid rating
                Comment = "Excellent resource!"
            };
            _reviewRepository.Setup(r => r.GetById(request.Id))
                .ReturnsAsync(review);
            // Act 
            var result = await _useCase.ExecuteAsync(request);
            // Assert 
            Assert.False(result.IsSuccess);
            Assert.Equal("InvalidInput", result.ErrorCode);
            _reviewRepository.Verify(r => r.UpdateAsync(It.IsAny<Review>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
            _logger.Verify(
               x => x.Log(
                   LogLevel.Warning,
                   It.IsAny<EventId>(),
                   It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("argumentos inválidos")),
                   It.IsAny<ArgumentException>(), // Puedes verificar el tipo de excepción
                   (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
               Times.Once);
        }
        [Fact]
        public async Task ExecuteAsync_PersistenceException_ReturnsFailure()
        {
            // Arrange
            var review = new Review
            {
                Id = Guid.NewGuid(),
                ResourceId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Rating = 4,
                Comment = "Good resource"
            };
            var request = new UpdateReviewRequest
            {
                Id = review.Id,
                Rating = 5,
                Comment = "Excellent resource!"
            };
            _reviewRepository.Setup(r => r.GetById(request.Id))
                .ReturnsAsync(review);
            _reviewRepository.Setup(r => r.UpdateAsync(It.IsAny<Review>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveAsync()).ThrowsAsync(new PersistenceException("Database error"));
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert 
            Assert.False(result.IsSuccess);
            Assert.Equal("PersistenceError", result.ErrorCode);
            _reviewRepository.Verify(r => r.UpdateAsync(It.IsAny<Review>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            _logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("error de persistencia")),
                    It.IsAny<PersistenceException>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }
        [Fact]
        public async Task ExecuteAsync_UnexpectedException_ReturnsFailure()
        {
            // Arrange
            var review = new Review
            {
                Id = Guid.NewGuid(),
                ResourceId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Rating = 4,
                Comment = "Good resource"
            };
            var request = new UpdateReviewRequest
            {
                Id = review.Id,
                Rating = 5,
                Comment = "Excellent resource!"
            };
            _reviewRepository.Setup(r => r.GetById(request.Id))
                .ReturnsAsync(review);
            _reviewRepository.Setup(r => r.UpdateAsync(It.IsAny<Review>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveAsync()).ThrowsAsync(new Exception("Database error"));
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert 
            Assert.False(result.IsSuccess);
            Assert.Equal("UnexpectedError", result.ErrorCode);
            _reviewRepository.Verify(r => r.UpdateAsync(It.IsAny<Review>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }
    }
}
