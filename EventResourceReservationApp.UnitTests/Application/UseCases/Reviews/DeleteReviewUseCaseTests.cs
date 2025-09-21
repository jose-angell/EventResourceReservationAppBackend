using Castle.Core.Logging;
using EventResourceReservationApp.Application.Common;
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
    public class DeleteReviewUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILogger<DeleteReviewUseCase>> _logger;
        private readonly Mock<IReviewRepository> _reviewRepository;
        private readonly DeleteReviewUseCase _useCase;
        public DeleteReviewUseCaseTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _logger = new Mock<ILogger<DeleteReviewUseCase>>();
            _reviewRepository = new Mock<IReviewRepository>();
            _mockUnitOfWork.Setup(u => u.Reviews).Returns(_reviewRepository.Object);
            _useCase = new DeleteReviewUseCase(_mockUnitOfWork.Object, _logger.Object);
        }
        [Fact]
        public async Task ExecuteAsync_WithValidRequest_ReturnsSuccess()
        {
            // Arrange
            var reviewId = Guid.NewGuid();
            var review = new Review
            {
                Id = reviewId,
                ResourceId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Rating = 4,
                Comment = "Good resource"
            };
            _reviewRepository.Setup(r => r.GetById(reviewId))
                .ReturnsAsync(review);
            _mockUnitOfWork.Setup(u => u.Reviews.RemoveASync(review))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveAsync())
                .Returns(Task.CompletedTask);
            // Act
            var result = await _useCase.ExecuteAsync(reviewId);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Reseña eliminada exitosamente.", result.Message);
            _logger.Verify(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Never);
        }
        [Fact]
        public async Task ExecuteAsync_WithNonExistentReview_ReturnsFailure()
        {
            // Arrange
            var reviewId = Guid.NewGuid();
            _reviewRepository.Setup(r => r.GetById(reviewId))
                .ReturnsAsync((Review?)null);
            // Act
            var result = await _useCase.ExecuteAsync(reviewId);
            // Assert 
            Assert.False(result.IsSuccess);
            Assert.Equal("NotFound", result.ErrorCode);

            _reviewRepository.Verify(r => r.GetById(reviewId), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Never);
            _logger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => o.ToString().Contains($"Reseña con Id {reviewId} no encontrada para actualización.")),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
            Times.Once);
        }
        [Fact]
        public async Task ExecuteAsync_PersistenceException_ReturnsFailure()
        {
            var reviewId = Guid.NewGuid();
            var review = new Review
            {
                Id = reviewId,
                ResourceId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Rating = 4,
                Comment = "Good resource"
            };
            _reviewRepository.Setup(r => r.GetById(reviewId))
                .ReturnsAsync(review);
            _mockUnitOfWork.Setup(u => u.Reviews.RemoveASync(review))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveAsync()).ThrowsAsync(new PersistenceException("Database error"));
            // Act 
            var result = await _useCase.ExecuteAsync(reviewId);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("PersistenceError", result.ErrorCode);
            _reviewRepository.Verify(r => r.GetById(reviewId), Times.Once);
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
            var reviewId = Guid.NewGuid();
            var review = new Review
            {
                Id = reviewId,
                ResourceId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Rating = 4,
                Comment = "Good resource"
            };
            _reviewRepository.Setup(r => r.GetById(reviewId))
                .ReturnsAsync(review);
            _mockUnitOfWork.Setup(u => u.Reviews.RemoveASync(review))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.SaveAsync()).ThrowsAsync(new Exception("Database error"));
            // Act 
            var result = await _useCase.ExecuteAsync(reviewId);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("UnexpectedError", result.ErrorCode);
            _reviewRepository.Verify(r => r.GetById(reviewId), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
            _logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("error inesperado")),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }
    }
}
