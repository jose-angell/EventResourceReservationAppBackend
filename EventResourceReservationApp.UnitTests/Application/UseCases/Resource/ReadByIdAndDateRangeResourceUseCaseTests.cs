using EventResourceReservationApp.Application.Common;
using EventResourceReservationApp.Application.DTOs.Resources;
using EventResourceReservationApp.Application.Repositories;
using EventResourceReservationApp.Application.UseCases.Resources;
using EventResourceReservationApp.Domain;
using EventResourceReservationApp.Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.UnitTests.Application.UseCases.Resource
{
    public class ReadByIdAndDateRangeResourceUseCaseTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IResourceRepository> _repository;
        private readonly Mock<ILogger<ReadByIdAndDateRangeResourceUseCase>> _logger;
        private readonly ReadByIdAndDateRangeResourceUseCase _useCase;
        public ReadByIdAndDateRangeResourceUseCaseTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repository = new Mock<IResourceRepository>();
            _logger = new Mock<ILogger<ReadByIdAndDateRangeResourceUseCase>>();
            _unitOfWork.Setup(u => u.Resources).Returns(_repository.Object);
            _useCase = new ReadByIdAndDateRangeResourceUseCase(_unitOfWork.Object, _logger.Object);
        }

        [Fact]
        public async Task ExecuteAsync_WithValidRequest_ReturnsSuccess()
        {
            // Arrange
            var request = new ReadByIdAndDateRangeResourceRequest
            {
                Id = Guid.NewGuid(),
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(1)
            };
            var existingResource = new ResourceResponse {
                Id = Guid.NewGuid(),
                StatusId = 1,
                StatusName = "",
                StatusDescription = "",
                Name = "",
                Description = "Description",
                Price = 21.98m,
                Quantity = 1,
                QuantityInUse = 1,
                CategoryId = 1,
                CategoryName =  "",
                LocationId = 1,
                LocationDescription = "",
                AuthorizationType = 1,
                Created = DateTime.UtcNow
            };
            _repository.Setup(r => r.GetByIdAsync(request.Id, request.StartTime, request.EndTime))
                .ReturnsAsync(existingResource);
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Recurso encontrado exitosamente.", result.Message);
        }
        [Fact]
        public async Task ExecuteAsync_WithNotFount_ReturnsFailure()
        {
            // Arrange
            var request = new ReadByIdAndDateRangeResourceRequest
            {
                Id = Guid.NewGuid(),
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(1)
            };
            _repository.Setup(r => r.GetByIdAsync(request.Id, request.StartTime, request.EndTime))
               .ReturnsAsync((ResourceResponse?)null);
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal($"La operación de busqueda falló debido a que no se encontró el recurso con Id '{request.Id}'.", result.Message);
        }
        [Fact]
        public async Task ExecuteAsync_WithPersistenceError_ReturnsFailure()
        {
            // Arrange
            var request = new ReadByIdAndDateRangeResourceRequest
            {
                Id = Guid.NewGuid(),
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(1)
            };
            var existingResource = new EventResourceReservationApp.Domain.Resource(1, "Projector", "HD Projector", 50, 150.89m, ResourceAuthorizationType.Automatico, 1, Guid.NewGuid());
            _repository.Setup(r => r.GetByIdAsync(request.Id, request.StartTime, request.EndTime))
                .ThrowsAsync(new PersistenceException("Database error"));
            // Act
            var result = await _useCase.ExecuteAsync(request);
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal($"La operación de lectura falló debido a un problema de almacenamiento de datos para el recurso con Id '{request.Id}'.", result.Message);
        }
    }
}
