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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.UnitTests.Application.UseCases.Reservations
{
    public class ReadAllReservationUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IReservationRepository> _repository;
        private readonly Mock<ILogger<ReadAllReservationUseCase>> _logger;
        private readonly ReadAllReservationUseCase _useCase;
        public ReadAllReservationUseCaseTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<IReservationRepository>();
            _logger = new Mock<ILogger<ReadAllReservationUseCase>>();
            _unitOfWork.Setup(u => u.Reservations).Returns(_repository.Object);
            _useCase = new ReadAllReservationUseCase(_unitOfWork.Object, _logger.Object);
        }
        private List<Reservation> GetTestLocations()
        {
            return new List<Reservation>
            {
                new Reservation(),
                new Reservation()
            };
        }
        [Fact]
        public async Task ExecuteAsync_WithoutFiltersOrOrdering_ReturnsAllReservations()
        {
            // Arrange
            var locations = GetTestLocations();
            var request = new ReadAllReservationRequest();
            _repository.Setup(r => r.GetAllAsync(
            It.IsAny<Expression<Func<Reservation, bool>>>(),
            It.IsAny<Func<IQueryable<Reservation>, IOrderedQueryable<Reservation>>>(),
            It.IsAny<string>()))
            .ReturnsAsync((Expression<Func<Reservation, bool>> filter, Func<IQueryable<Reservation>, IOrderedQueryable<Reservation>> orderBy, string include) =>
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
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task ExecuteAsync_WithPersistenceException_ReturnsFailure()
        {
            // Arrange
            var request = new ReadAllReservationRequest();
            _repository.Setup(r => r.GetAllAsync(
                It.IsAny<Expression<Func<Reservation, bool>>>(),
                It.IsAny<Func<IQueryable<Reservation>, IOrderedQueryable<Reservation>>>(),
                It.IsAny<string>()))
                .Throws(new PersistenceException("La operación de lectura falló debido a un problema de almacenamiento de datos."));
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("La operación de consulta falló debido a un problema de almacenamiento de datos.", result.Message);
        }
    }
}
