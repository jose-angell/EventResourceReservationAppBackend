using Castle.Core.Logging;
using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Locations;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Application.UseCases.ReservationCarItems;
using EventResourceReservationApp.Domain;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.UnitTests.Application.UseCases.ReservationCarItems
{
    public class ReadAllReservationCarItemUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILogger<ReadAllReservationCarItemUseCase>> _mockLogger;
        private readonly Mock<IReservationCarItemRepository> _mockReservationCarItemRepository;
        private readonly ReadAllReservationCarItemUseCase _useCase;
        public ReadAllReservationCarItemUseCaseTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<ReadAllReservationCarItemUseCase>>();
            _mockReservationCarItemRepository = new Mock<IReservationCarItemRepository>();
            _mockUnitOfWork.Setup(u => u.ReservationCarItems).Returns(_mockReservationCarItemRepository.Object);
            _useCase = new ReadAllReservationCarItemUseCase(_mockUnitOfWork.Object, _mockLogger.Object);
        }
        public List<ReservationCarItem> GetTestsReservationCarItems()
        {
            return new List<ReservationCarItem>
            {
                new ReservationCarItem { Id = new Guid("11111111-1111-1111-1111-111111111111"), ClientId = new Guid("11111111-1111-1111-1111-111111111111") , ResourceId = new Guid("11111111-1111-1111-1111-111111111111"), Quantity = 1, AddedAt = new DateTime(2025, 1, 1) },
                new ReservationCarItem { Id = new Guid("22222222-2222-2222-2222-222222222222"), ClientId = new Guid("22222222-2222-2222-2222-222222222222") , ResourceId = new Guid("22222222-2222-2222-2222-222222222222"), Quantity = 1, AddedAt = new DateTime(2025, 1, 2) },
                new ReservationCarItem { Id = new Guid("33333333-3333-3333-3333-333333333333"), ClientId = new Guid("33333333-3333-3333-3333-333333333333") , ResourceId = new Guid("33333333-3333-3333-3333-333333333333"), Quantity = 1, AddedAt = new DateTime(2025, 1, 3) }
            };
        }
        [Fact]
        public async Task ExecuteAsync_WithValidRequest_RetunrsSuccess()
        {
            // Arrange
            var itemId = new Guid("11111111-1111-1111-1111-111111111111");
            var ReservationCarItems = GetTestsReservationCarItems();
            _mockReservationCarItemRepository.Setup(r => r.GetAllAsync(It.IsAny<Expression<Func<ReservationCarItem, bool>>>(),
            It.IsAny<Func<IQueryable<ReservationCarItem>, IOrderedQueryable<ReservationCarItem>>>(),
            It.IsAny<string>()))
            .ReturnsAsync((Expression<Func<ReservationCarItem, bool>> filter, Func<IQueryable<ReservationCarItem>, IOrderedQueryable<ReservationCarItem>> orderBy, string include) =>
            {
                var query = ReservationCarItems.AsQueryable();
                if (filter != null)
                {
                    query = query.Where(filter);
                }
                return query.ToList();
            });
            // Act
            var result = await _useCase.ExecuteAsync(itemId);

            // Assert

            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
        [Fact]
        public async Task ExecuteAsync_WithPersistenceException_ReturnsFailure()
        {
            // Arrange
            var request = new Guid("11111111-1111-1111-1111-111111111111");
            _mockReservationCarItemRepository.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<ReservationCarItem, bool>>>(),
                It.IsAny<Func<IQueryable<ReservationCarItem>, IOrderedQueryable<ReservationCarItem>>>(),
                It.IsAny<string>()))
                .Throws(new PersistenceException("La operación de lectura falló debido a un problema de almacenamiento de datos."));
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("PersistenceError", result.ErrorCode);
            Assert.Equal("La operación de lectura falló debido a un problema de almacenamiento de datos.", result.Message);
        }
        [Fact]
        public async Task ExecuteAsync_WithUxpectedException_ReturnsFailure()
        {
            // Arrange
            var request = new Guid("11111111-1111-1111-1111-111111111111");
            _mockReservationCarItemRepository.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<ReservationCarItem, bool>>>(),
                It.IsAny<Func<IQueryable<ReservationCarItem>, IOrderedQueryable<ReservationCarItem>>>(),
                It.IsAny<string>()))
                .Throws(new Exception("La operación de lectura falló debido a un problema de almacenamiento de datos."));
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("UnexpectedError", result.ErrorCode);
            Assert.Equal("La operación de lectura falló debido a un problema inesperado.", result.Message);
        }
    }
}
