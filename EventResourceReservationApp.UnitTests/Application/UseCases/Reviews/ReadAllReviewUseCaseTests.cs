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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.UnitTests.Application.UseCases.Reviews
{
    public class ReadAllReviewUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IReviewRepository> _reviewRepository;
        private readonly ReadAllReviewUseCase _useCase;
        private readonly Mock<ILogger<ReadAllReviewUseCase>> _logger;
        public ReadAllReviewUseCaseTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _reviewRepository = new Mock<IReviewRepository>();
            _logger = new Mock<ILogger<ReadAllReviewUseCase>>();
            _mockUnitOfWork.Setup(u => u.Reviews).Returns(_reviewRepository.Object);
            _useCase = new ReadAllReviewUseCase(_mockUnitOfWork.Object, _logger.Object);
        }
        private List<Review> GetTestReviews()
        {
            return new List<Review>
            {
                new Review { Id = new Guid("11111111-1111-1111-1111-111111111111"), ResourceId = new Guid("11111111-1111-1111-1111-111111111111") , UserId = new Guid("11111111-1111-1111-1111-111111111111"), Rating = 1, CreatedAt = new DateTime(2025, 1, 1)  },
                new Review { Id = new Guid("22222222-2222-2222-2222-222222222222"), ResourceId = new Guid("22222222-2222-2222-2222-222222222222") , UserId = new Guid("22222222-2222-2222-2222-222222222222"), Rating = 5, CreatedAt = new DateTime(2025, 1, 2)  },
                new Review { Id = new Guid("33333333-3333-3333-3333-333333333333"), ResourceId = new Guid("33333333-3333-3333-3333-333333333333") , UserId = new Guid("33333333-3333-3333-3333-333333333333"), Rating = 4, CreatedAt = new DateTime(2025, 1, 6)  },
            };
        }
        [Fact]
        public async Task ExecuteAsync_WithValidRequest_ReturnsSuccess()
        {
            // Arrange
            var reviews = GetTestReviews();
            var request = new ReadReviewRequest();
            _reviewRepository.Setup(r => r.GetAllAsync(
            It.IsAny<Expression<Func<Review, bool>>>(),
            It.IsAny<Func<IQueryable<Review>, IOrderedQueryable<Review>>>(),
            It.IsAny<string>()))
            .ReturnsAsync((Expression<Func<Review, bool>> filter, Func<IQueryable<Review>, IOrderedQueryable<Review>> orderBy, string include) =>
            {
                var query = reviews.AsQueryable();
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                if (orderBy != null)
                {
                    return orderBy(query).ToList();
                }
                return query.ToList();
            });
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(3, result.Data.Count());
        }
        [Fact]
        public async Task ExecuteAsync_WithValidRequest_UserIdFilter_ReturnsSuccess()
        {
            // Arrange
            var reviews = GetTestReviews();
            var request = new ReadReviewRequest
            {
                UserId = new Guid("11111111-1111-1111-1111-111111111111")
            };
            _reviewRepository.Setup(r => r.GetAllAsync(
            It.IsAny<Expression<Func<Review, bool>>>(),
            It.IsAny<Func<IQueryable<Review>, IOrderedQueryable<Review>>>(),
            It.IsAny<string>()))
            .ReturnsAsync((Expression<Func<Review, bool>> filter, Func<IQueryable<Review>, IOrderedQueryable<Review>> orderBy, string include) =>
            {
                var query = reviews.AsQueryable();
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                if (orderBy != null)
                {
                    return orderBy(query).ToList();
                }
                return query.ToList();
            });
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Data.Count());
        }
        [Fact]
        public async Task ExecuteAsync_WithValidRequest_rating_desc_Orderby_ReturnsSuccess()
        {
            // Arrange
            var reviews = GetTestReviews();
            var request = new ReadReviewRequest
            {
                OrderBy = "rating_desc"
            };
            _reviewRepository.Setup(r => r.GetAllAsync(
            It.IsAny<Expression<Func<Review, bool>>>(),
            It.IsAny<Func<IQueryable<Review>, IOrderedQueryable<Review>>>(),
            It.IsAny<string>()))
            .ReturnsAsync((Expression<Func<Review, bool>> filter, Func<IQueryable<Review>, IOrderedQueryable<Review>> orderBy, string include) =>
            {
                var query = reviews.AsQueryable();
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                if (orderBy != null)
                {
                    return orderBy(query).ToList();
                }
                return query.ToList();
            });
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(3, result.Data.Count());
            Assert.Equal(5, result.Data.First().Rating);
        }
        [Fact]
        public async Task ExecuteAsync_PersistenceException_ReturnsFailure()
        {
            // Arrange
            var request = new ReadReviewRequest();
            _reviewRepository.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Review, bool>>>(),
                It.IsAny<Func<IQueryable<Review>, IOrderedQueryable<Review>>>(),
                It.IsAny<string>()))
                .ThrowsAsync(new PersistenceException("Database error"));
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("PersistenceError", result.ErrorCode);
        }
        [Fact]
        public async Task ExecuteAsync_UnexpectedException_ReturnsFailure()
        {
            // Arrange
            var request = new ReadReviewRequest();
            _reviewRepository.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Review, bool>>>(),
                It.IsAny<Func<IQueryable<Review>, IOrderedQueryable<Review>>>(),
                It.IsAny<string>()))
                .ThrowsAsync(new Exception("Database error"));
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("UnexpectedError", result.ErrorCode);
        }
    }
}
