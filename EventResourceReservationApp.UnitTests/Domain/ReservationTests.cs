using EventResourceReservationApp.Domain;
using EventResourceReservationApp.Domain.Enums;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.UnitTests.Domain
{
    public class ReservationTests
    {
        [Fact] 
        public void Constructor_WithValidParameters_InitializesCorrectly()
        {
            //Arrange
            var clientId = Guid.NewGuid();
            var startTime = DateTime.UtcNow;
            var endTime = startTime.AddHours(2);
            decimal totalAmount = 10m;
            string clientComment = "commentClient";
            string clientPhoneNumber = "3124512";
            int locationId = 1;
            
            //Act
            var reservation = new Reservation(startTime, endTime, totalAmount, clientComment, clientPhoneNumber, locationId, clientId);

            //Assert
            Assert.Equal(clientId, reservation.ClientId);
            Assert.Equal(startTime, reservation.StartTime);
            Assert.Equal(endTime, reservation.EndTime);
        }
        [Fact]
        public void Constructor_WithInvalidDateTime_ThrowsArgumentException()
        {
            //Arrange
            var clientId = Guid.NewGuid();
            var startTime = DateTime.UtcNow;
            var endTime = startTime;
            decimal totalAmount = 10m;
            string clientComment = "commentClient";
            string clientPhoneNumber = "3124512";
            int locationId = 1;

            //Act  and Assert
            Assert.Throws<ArgumentException>(() => new Reservation(startTime, endTime, totalAmount, clientComment, clientPhoneNumber, locationId, clientId));
        }
        [Fact]
        public void Constructor_WithInvalidTotalAmounte_ThrowsArgumentException()
        {
            //Arrange
            var clientId = Guid.NewGuid();
            var startTime = DateTime.UtcNow;
            var endTime = startTime.AddHours(2);
            decimal totalAmount = 0m;
            string clientComment = "commentClient";
            string clientPhoneNumber = "3124512";
            int locationId = 1;

            //Act  and Assert
            Assert.Throws<ArgumentException>(() => new Reservation(startTime, endTime, totalAmount, clientComment, clientPhoneNumber, locationId, clientId));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_WithNullOrEmptyclientPhoneNumber_ThrowsArgumentException(string clientPhoneNumber)
        {
            //Arrange
            var clientId = Guid.NewGuid();
            var startTime = DateTime.UtcNow;
            var endTime = startTime.AddHours(2);
            decimal totalAmount = 0m;
            string clientComment = "commentClient";
            int locationId = 1;

            //Act  and Assert
            Assert.Throws<ArgumentException>(() => new Reservation(startTime, endTime, totalAmount, clientComment, clientPhoneNumber, locationId, clientId));
        }

        [Fact]
        public void Constructor_WithInvalidLocationId_ThrowsArgumentException()
        {
            //Arrange
            var clientId = Guid.NewGuid();
            var startTime = DateTime.UtcNow;
            var endTime = startTime.AddHours(2);
            decimal totalAmount = 10m;
            string clientComment = "commentClient";
            string clientPhoneNumber = "3124512";
            int locationId = 0;

            //Act  and Assert
            Assert.Throws<ArgumentException>(() => new Reservation(startTime, endTime, totalAmount, clientComment, clientPhoneNumber, locationId, clientId));
        }

        [Fact]
        public void Constructor_WithInvaliClientId_ThrowsArgumentException()
        {
            //Arrange
            var clientId = Guid.Empty;
            var startTime = DateTime.UtcNow;
            var endTime = startTime.AddHours(2);
            decimal totalAmount = 10m;
            string clientComment = "commentClient";
            string clientPhoneNumber = "3124512";
            int locationId = 1;

            //Act  and Assert
            Assert.Throws<ArgumentException>(() => new Reservation(startTime, endTime, totalAmount, clientComment, clientPhoneNumber, locationId, clientId));
        }

        [Fact]
        public void Update_WithValidParameters_UpdatesCorrectly()
        {
            var startTime = DateTime.UtcNow;
            var endTime = startTime.AddHours(2);
            var reservation = new Reservation(startTime, endTime, 12m,"commentClient","1241234", 1, Guid.NewGuid());
            var newStartTime = startTime.AddHours(1);
            var newEndTime = endTime.AddHours(1);
            var newStatusId = ReservationStatus.Confirmed;
            decimal newTotalAmount = 20m;
            var newClientComment = "newCommentClient";
            var newClientPhoneNumber = "567890";
            int newLocationId = 2;

            //Act
            reservation.Update(newStartTime, newEndTime, newStatusId, newTotalAmount, newClientComment, newClientPhoneNumber, newLocationId);

            //Assert
            Assert.Equal(newStartTime, reservation.StartTime);
            Assert.Equal(newEndTime, reservation.EndTime);
            Assert.Equal(newStatusId, reservation.StatusId);
            Assert.Equal(newTotalAmount, reservation.TotalAmount);
            Assert.Equal(newClientComment, reservation.ClientComment);
            Assert.Equal(newClientPhoneNumber, reservation.ClientPhoneNumber);
            Assert.Equal(newLocationId, reservation.LocationId);

        }
        [Fact]
        public void Update_WithInvalidDateTime_ThrowsArgumentException()
        {
            var startTime = DateTime.UtcNow;
            var endTime = startTime.AddHours(2);
            var reservation = new Reservation(startTime, endTime, 12m, "commentClient", "1241234", 1, Guid.NewGuid());
            var newStartTime = startTime.AddHours(1);
            var newEndTime = newStartTime;
            var newStatusId = ReservationStatus.Confirmed;
            decimal newTotalAmount = 20m;
            var newClientComment = "newCommentClient";
            var newClientPhoneNumber = "567890";
            int newLocationId = 2;

            //Act  and Assert
            Assert.Throws<ArgumentException>(() => reservation.Update(newStartTime, newEndTime, newStatusId, newTotalAmount, newClientComment, newClientPhoneNumber, newLocationId));

        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Update_WithNullOrEmptyclientPhoneNumber_ThrowsArgumentException(string newClientPhoneNumber)
        {
            var startTime = DateTime.UtcNow;
            var endTime = startTime.AddHours(2);
            var reservation = new Reservation(startTime, endTime, 12m, "commentClient", "1241234", 1, Guid.NewGuid());
            var newStartTime = startTime.AddHours(1);
            var newEndTime = endTime.AddHours(1);
            var newStatusId = ReservationStatus.Confirmed;
            decimal newTotalAmount = 20m;
            var newClientComment = "newCommentClient";
            int newLocationId = 2;

            //Act and Assert
            Assert.Throws<ArgumentException>(() => reservation.Update(newStartTime, newEndTime, newStatusId, newTotalAmount, newClientComment, newClientPhoneNumber, newLocationId));

        }

        [Fact]
        public void Update_WithInvalidLocationId_ThrowsArgumentException()
        {
            var startTime = DateTime.UtcNow;
            var endTime = startTime.AddHours(2);
            var reservation = new Reservation(startTime, endTime, 12m, "commentClient", "1241234", 1, Guid.NewGuid());
            var newStartTime = startTime.AddHours(1);
            var newEndTime = endTime.AddHours(1);
            var newStatusId = ReservationStatus.Confirmed;
            decimal newTotalAmount = 20m;
            var newClientComment = "newCommentClient";
            var newClientPhoneNumber = "567890";
            int newLocationId = 0;

            //Act  and Assert
            Assert.Throws<ArgumentException>(() => reservation.Update(newStartTime, newEndTime, newStatusId, newTotalAmount, newClientComment, newClientPhoneNumber, newLocationId));

        }

        [Fact]
        public void Update_WithInvalidTotalAmount_ThrowsArgumentException()
        {
            var startTime = DateTime.UtcNow;
            var endTime = startTime.AddHours(2);
            var reservation = new Reservation(startTime, endTime, 12m, "commentClient", "1241234", 1, Guid.NewGuid());
            var newStartTime = startTime.AddHours(1);
            var newEndTime = endTime.AddHours(1);
            var newStatusId = ReservationStatus.Confirmed;
            decimal newTotalAmount = 0m;
            var newClientComment = "newCommentClient";
            var newClientPhoneNumber = "567890";
            int newLocationId = 2;

            //Act  and Assert
            Assert.Throws<ArgumentException>(() => reservation.Update(newStartTime, newEndTime, newStatusId, newTotalAmount, newClientComment, newClientPhoneNumber, newLocationId));

        }
        [Fact]
        public void UpdateStatus_WithValidParameters_UpdatesCorrectly()
        {
            var startTime = DateTime.UtcNow;
            var endTime = startTime.AddHours(2);
            var reservation = new Reservation(startTime, endTime, 12m, "commentClient", "1241234", 1, Guid.NewGuid());
            Guid adminId = Guid.NewGuid(); 
            string adminComment = "adminComment";
            var newStatusId = ReservationStatus.Confirmed;

            //Act
            reservation.UpdateStatus(newStatusId, adminId, adminComment);

            //Assert
            Assert.Equal(newStatusId, reservation.StatusId);
            Assert.True(reservation.AdminId.HasValue);
        }
        [Fact]
        public void UpdateStatus_WithInvalidStatusId_ThrowsArgumentException()
        {
            var startTime = DateTime.UtcNow;
            var endTime = startTime.AddHours(2);
            var reservation = new Reservation(startTime, endTime, 12m, "commentClient", "1241234", 1, Guid.NewGuid());
            Guid adminId = Guid.NewGuid();
            string adminComment = "adminComment";
            var newStatusId = (ReservationStatus)0;

            //Act  and Assert
            Assert.Throws<ArgumentException>(() => reservation.UpdateStatus(newStatusId, adminId, adminComment));

        }
        [Fact]
        public void UpdateStatus_WithInvalidAdminId_ThrowsArgumentException()
        {
            var startTime = DateTime.UtcNow;
            var endTime = startTime.AddHours(2);
            var reservation = new Reservation(startTime, endTime, 12m, "commentClient", "1241234", 1, Guid.NewGuid());
            Guid adminId = Guid.Empty;
            string adminComment = "adminComment";
            var newStatusId = ReservationStatus.Confirmed;

            //Act  and Assert
            Assert.Throws<ArgumentException>(() => reservation.UpdateStatus(newStatusId, adminId, adminComment));

        }

        [Fact]
        public void UpdateTransaction_WithValidParameters_UpdatesCorrectly()
        {
            var startTime = DateTime.UtcNow;
            var endTime = startTime.AddHours(2);
            var reservation = new Reservation(startTime, endTime, 12m, "commentClient", "1241234", 1, Guid.NewGuid());
            Guid transationId = Guid.NewGuid();
            var newStatusId = ReservationStatus.Confirmed;

            //Act
            reservation.UpdateTransaction(newStatusId, transationId);

            //Assert
            Assert.Equal(newStatusId, reservation.StatusId);
            Assert.True(reservation.TransactionId.HasValue);
        }
        [Fact]
        public void UpdateTransaction_WithInvalidStatusId_ThrowsArgumentException()
        {
            var startTime = DateTime.UtcNow;
            var endTime = startTime.AddHours(2);
            var reservation = new Reservation(startTime, endTime, 12m, "commentClient", "1241234", 1, Guid.NewGuid());
            Guid transationId = Guid.NewGuid();
            var newStatusId = (ReservationStatus)0;

            //Act  and Assert
            Assert.Throws<ArgumentException>(() => reservation.UpdateTransaction(newStatusId, transationId));

        }
        [Fact]
        public void UpdateTransaction_WithInvalidTransactionId_ThrowsArgumentException()
        {
            var startTime = DateTime.UtcNow;
            var endTime = startTime.AddHours(2);
            var reservation = new Reservation(startTime, endTime, 12m, "commentClient", "1241234", 1, Guid.NewGuid());
            Guid transationId = Guid.Empty;
            var newStatusId = ReservationStatus.Confirmed;

            //Act  and Assert
            Assert.Throws<ArgumentException>(() => reservation.UpdateTransaction(newStatusId, transationId));

        }
    }
}
