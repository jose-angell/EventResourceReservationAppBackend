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
    public class CreateReviewUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<ILogger<CreateReviewUseCase>> _logger;
        private readonly Mock<IReviewRepository> _reviewRepository;
        private readonly CreateReviewUseCase _useCase;
        public CreateReviewUseCaseTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _logger = new Mock<ILogger<CreateReviewUseCase>>();
            _reviewRepository = new Mock<IReviewRepository>();
            _unitOfWork.Setup(u => u.Reviews).Returns(_reviewRepository.Object);
            _useCase = new CreateReviewUseCase(_unitOfWork.Object, _logger.Object);
        }
        [Fact]
        public async Task ExecuteAsync_WithValidRequest_ReturnsSuccess()
        {
            // Arrange
            var request = new CreateReviewRequest
            {
                ResourceId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Rating = 5,
                Comment = "Great resource!"
            };
            _reviewRepository.Setup(r => r.AddAsync(It.IsAny<Review>()))
                .Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.SaveAsync())
                .Returns(Task.CompletedTask);
            // Act 
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(request.ResourceId, result.Data.ResourceId);
            Assert.Equal(request.UserId, result.Data.UserId);
            Assert.Equal(request.Rating, result.Data.Rating);
            Assert.Equal(request.Comment, result.Data.Comment);
            _reviewRepository.Verify(r => r.AddAsync(It.IsAny<Review>()), Times.Once);
            _unitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }
        [Fact]
        public async Task ExecuteAsync_WithInvalidRating_ReturnsFailure()
        {
            // Arrange
            var request = new CreateReviewRequest
            {
                ResourceId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Rating = 6, // Invalid rating
                Comment = "Great resource!"
            };
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal("InvalidInput", result.ErrorCode);
            _reviewRepository.Verify(r => r.AddAsync(It.IsAny<Review>()), Times.Never);
            _unitOfWork.Verify(u => u.SaveAsync(), Times.Never);
        }
        [Fact]
        public async Task ExecuteAsync_WhenPersistenceFails_ReturnsFailure()
        {
            // Arrange
            var request = new CreateReviewRequest
            {
                ResourceId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Rating = 5,
                Comment = "Great resource!"
            };
            _reviewRepository.Setup(r => r.AddAsync(It.IsAny<Review>()))
                .Returns(Task.CompletedTask);
            _unitOfWork.Setup(u => u.SaveAsync())
                .ThrowsAsync(new PersistenceException("Database error"));
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Data);
            Assert.Equal("PersistenceError", result.ErrorCode);
            _reviewRepository.Verify(r => r.AddAsync(It.IsAny<Review>()), Times.Once);
            _unitOfWork.Verify(u => u.SaveAsync(), Times.Once);
        }
    }
}
