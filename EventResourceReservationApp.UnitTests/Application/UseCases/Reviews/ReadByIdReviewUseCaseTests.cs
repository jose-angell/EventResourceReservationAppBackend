﻿using Castle.Core.Logging;
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
    public class ReadByIdReviewUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILogger<ReadByIdReviewUseCase>> _logger;
        private readonly Mock<IReviewRepository> _reviewRepository;
        private readonly ReadByIdReviewUseCase _useCase;
        public ReadByIdReviewUseCaseTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _logger = new Mock<ILogger<ReadByIdReviewUseCase>>();
            _reviewRepository = new Mock<IReviewRepository>();
            _mockUnitOfWork.Setup(u => u.Reviews).Returns(_reviewRepository.Object);
            _useCase = new ReadByIdReviewUseCase(_mockUnitOfWork.Object, _logger.Object);
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
            // Act 
            var result = await _useCase.ExecuteAsync(reviewId);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.Equal(reviewId, result.Data.Id);
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
        }
        [Fact]
        public async Task ExecuteAsync_PersistenceException_ReturnsFailure()
        {
            // Arrange
            var reviewId = Guid.NewGuid();
            _reviewRepository.Setup(r => r.GetById(reviewId)).ThrowsAsync(new PersistenceException("Database error"));

            // Act
            var result = await _useCase.ExecuteAsync(reviewId);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("PersistenceError", result.ErrorCode);
        }
        [Fact]
        public async Task ExecuteAsync_UnexpectedException_ReturnsFailure()
        {
            // Arrange
            var reviewId = Guid.NewGuid();
            _reviewRepository.Setup(r => r.GetById(reviewId)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _useCase.ExecuteAsync(reviewId);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("UnexpectedError", result.ErrorCode);
        }
    }
}
