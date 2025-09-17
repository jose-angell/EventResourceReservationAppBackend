using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Locations;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Application.UseCases.Locations;
using EventResourceReservationApp.Domain;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.UnitTests.Application.UseCases.Locations
{
    public class ReadAllLocationUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILocationRepository> _mockLocationRepository;
        private readonly ReadAllLocationUseCase _useCase;
        private readonly Mock<ILogger<ReadAllLocationUseCase>> _mockLogger;
        public ReadAllLocationUseCaseTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLocationRepository = new Mock<ILocationRepository>();
            _mockLogger = new Mock<ILogger<ReadAllLocationUseCase>>();
            _mockUnitOfWork.Setup(u => u.Locations).Returns(_mockLocationRepository.Object);
            _useCase = new ReadAllLocationUseCase(_mockUnitOfWork.Object, _mockLogger.Object);
        }
        private List<Location> GetTestLocations()
        {
            return new List<Location>
            {
                new Location {Country = "Country 1",City = "City 1", ZipCode = 12345, Street = "Street 1", Neighborhood = "Neighborhood 1",ExteriorNumber = "123",InteriorNumber = "A", CreatedByUserId = new Guid("11111111-1111-1111-1111-111111111111"), CreatedAt = new DateTime(2025, 1, 1) },
                new Location {Country = "Country 2",City = "City 2", ZipCode = 54321, Street = "Street 2", Neighborhood = "Neighborhood 2",ExteriorNumber = "456",InteriorNumber = "B", CreatedByUserId = new Guid("22222222-2222-2222-2222-222222222222"), CreatedAt = new DateTime(2025, 1, 3) },
                new Location {Country = "Country 3",City = "City 3", ZipCode = 67890, Street = "Street 3", Neighborhood = "Neighborhood 3",ExteriorNumber = "789",InteriorNumber = "C", CreatedByUserId = new Guid("33333333-3333-3333-3333-333333333333"), CreatedAt = new DateTime(2025, 1, 4)  }
            };
        }
        [Fact] 
        public async Task ExecuteAsync_WithoutFiltersOrOrdering_ReturnsAllLocations()
        {
            // Arrange
            var locations = GetTestLocations();
            var request = new ReadAllLocationRequest();
            _mockLocationRepository.Setup(r => r.GetAllAsync(
            It.IsAny<Expression<Func<Location, bool>>>(),
            It.IsAny<Func<IQueryable<Location>, IOrderedQueryable<Location>>>(),
            It.IsAny<string>()))
            .ReturnsAsync((Expression<Func<Location, bool>> filter, Func<IQueryable<Location>, IOrderedQueryable<Location>> orderBy, string include) =>
            {
                var query = locations.AsQueryable();
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
            Assert.NotNull(result.Data);
            Assert.Equal(3, result.Data.Count());
        }
        [Fact]
        public async Task ExecuteAsync_WithFilterByCity_ReturnsFilteredCategories()
        {
            var locations = GetTestLocations();
            var request = new ReadAllLocationRequest { City = "City 1" };
            _mockLocationRepository.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Location, bool>>>(),
                It.IsAny<Func<IQueryable<Location>, IOrderedQueryable<Location>>>(),
                It.IsAny<string>()))
                .ReturnsAsync((Expression<Func<Location, bool>> filter, Func<IQueryable<Location>, IOrderedQueryable<Location>> orderBy, string include) =>
                {
                    var query = locations.AsQueryable();
                    if (filter != null)
                    {
                        query = query.Where(filter);
                    }
                    return query.ToList();
                });

            // Act
            var result = await _useCase.ExecuteAsync(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Data);
            Assert.Equal("City 1", result.Data.First().City);
        }
        [Fact]
        public async Task ExecuteAsync_WithFilterByZipCode_ReturnsFilteredCategories()
        {
            var locations = GetTestLocations();
            var request = new ReadAllLocationRequest { ZipCode = 12345 };
            _mockLocationRepository.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Location, bool>>>(),
                It.IsAny<Func<IQueryable<Location>, IOrderedQueryable<Location>>>(),
                It.IsAny<string>()))
                .ReturnsAsync((Expression<Func<Location, bool>> filter, Func<IQueryable<Location>, IOrderedQueryable<Location>> orderBy, string include) =>
                {
                    var query = locations.AsQueryable();
                    if (filter != null)
                    {
                        query = query.Where(filter);
                    }
                    return query.ToList();
                });
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Data);
            Assert.Equal(12345, result.Data.First().ZipCode);
        }
        [Fact]
        public async Task ExecuteAsync_WithFilterByCreatedByUserId_ReturnsFilteredCategories()
        {
            var locations = GetTestLocations();
            var userId = new Guid("33333333-3333-3333-3333-333333333333");
            var request = new ReadAllLocationRequest { CreatedByUserIdFilter = userId };
            _mockLocationRepository.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Location, bool>>>(),
                It.IsAny<Func<IQueryable<Location>, IOrderedQueryable<Location>>>(),
                It.IsAny<string>()))
                .ReturnsAsync((Expression<Func<Location, bool>> filter, Func<IQueryable<Location>, IOrderedQueryable<Location>> orderBy, string include) =>
                {
                    var query = locations.AsQueryable();
                    if (filter != null)
                    {
                        query = query.Where(filter);
                    }
                    return query.ToList();
                });
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Data);
            Assert.Equal(userId, result.Data.First().CreatedByUserId);
        }
        [Fact]
        public async Task ExecuteAsync_WithOrderingByCity_ReturnsOrderedLocations()
        {
            var locations = GetTestLocations();
            var request = new ReadAllLocationRequest { OrderBy = "city_asc" };
            _mockLocationRepository.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Location, bool>>>(),
                It.IsAny<Func<IQueryable<Location>, IOrderedQueryable<Location>>>(),
                It.IsAny<string>()))
                .ReturnsAsync((Expression<Func<Location, bool>> filter, Func<IQueryable<Location>, IOrderedQueryable<Location>> orderBy, string include) =>
                {
                    var query = locations.AsQueryable();
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
            Assert.Equal("City 1", result.Data.First().City);
            Assert.Equal("City 2", result.Data.Skip(1).First().City);
        }
        [Fact]
        public async Task ExecuteAsync_WithOrderingByCreatedAt_ReturnsOrderedLocations()
        {
            var locations = GetTestLocations();
            var request = new ReadAllLocationRequest { OrderBy = "createdAt_asc" };
            _mockLocationRepository.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Location, bool>>>(),
                It.IsAny<Func<IQueryable<Location>, IOrderedQueryable<Location>>>(),
                It.IsAny<string>()))
                .ReturnsAsync((Expression<Func<Location, bool>> filter, Func<IQueryable<Location>, IOrderedQueryable<Location>> orderBy, string include) =>
                {
                    var query = locations.AsQueryable();
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
            Assert.Equal(new DateTime(2025, 1, 4), result.Data.First().CreatedAt);
            Assert.Equal(new DateTime(2025, 1, 3), result.Data.Skip(1).First().CreatedAt);
        }
        [Fact]
        public async Task ExecuteAsync_WithPersistenceException_ReturnsFailure()
        {
            // Arrange
            var request = new ReadAllLocationRequest();
            _mockLocationRepository.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Location, bool>>>(),
                It.IsAny<Func<IQueryable<Location>, IOrderedQueryable<Location>>>(),
                It.IsAny<string>()))
                .Throws(new PersistenceException("La operación de lectura falló debido a un problema de almacenamiento de datos."));
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("La operación de lectura falló debido a un problema de almacenamiento de datos.", result.Message);
        }
    }
}
