using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventResourceReservationApp.Domain
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int StatusId { get; set; }
        public decimal TotalAmount { get; set; }
        public string ClientComment { get; set; }
        public string ClientPhoneNumber { get; set; }   
        public int LocationId { get; set; }
        public Location? Location { get; set; }
        public Guid ClientId { get; set; }
        public ApplicationUser? Client { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? AdminId { get; set; }
        public ApplicationUser? Admin { get; set; }
        public string? AdminComment { get; set; }
        public Guid? TransactionId { get; set; }
        public ICollection<ReservationDetail>? ReservationDetail { get; set; }
        public Reservation()
        {
            ClientComment = string.Empty;
            ClientPhoneNumber = string.Empty;
            AdminComment = string.Empty;
        }
        public Reservation(DateTime startTime, DateTime endTime, decimal totalAmount, 
            string? clientComment, string clientPhoneNumber, int locationId, Guid clientId)
        {
            if(startTime >= endTime)
            {
                throw new ArgumentException("StartTime must be earlier than EndTime.", nameof(startTime));
            }
            if(totalAmount <= 0)
            {
                throw new ArgumentException("TotalAmount must be a positive value.", nameof(totalAmount));
            }
            if (string.IsNullOrWhiteSpace(clientPhoneNumber))
            {
                throw new ArgumentException("ClientPhoneNumber cannot be null or empty.", nameof(clientPhoneNumber));
            }
            if (locationId == 0)
            {
                throw new ArgumentException("LocationId cannot be empty.", nameof(locationId));
            }
            if (clientId == Guid.Empty)
            {
                throw new ArgumentException("ClientId cannot be empty.", nameof(clientId));
            }
            StartTime = startTime;
            EndTime = endTime;
            StatusId = 1;
            TotalAmount = totalAmount;
            ClientComment = clientComment ?? string.Empty;
            ClientPhoneNumber = clientPhoneNumber;
            LocationId = locationId;
            ClientId = clientId;
            AdminComment = string.Empty;
            CreatedAt = DateTime.UtcNow;
        }
        public void Update(DateTime startTime, DateTime endTime, int statusId, decimal totalAmount,
            string clientComment, string clientPhoneNumber, int locationId)
        {
            if (startTime >= endTime)
            {
                throw new ArgumentException("StartTime must be earlier than EndTime.", nameof(startTime));
            }
            if (statusId <= 0)
            {
                throw new ArgumentException("StatusId cannot be empty.", nameof(statusId));
            }
            if (totalAmount <= 0)
            {
                throw new ArgumentException("TotalAmount must be a positive value.", nameof(totalAmount));
            }
            if (string.IsNullOrWhiteSpace(clientPhoneNumber))
            {
                throw new ArgumentException("ClientPhoneNumber cannot be null or empty.", nameof(clientPhoneNumber));
            }
            if (locationId == 0)
            {
                throw new ArgumentException("LocationId cannot be empty.", nameof(locationId));
            }
            StartTime = startTime;
            EndTime = endTime;
            StatusId = statusId;
            TotalAmount = totalAmount;
            ClientComment = clientComment ?? string.Empty;
            ClientPhoneNumber = clientPhoneNumber;
            LocationId = locationId;
        }
        public void UpdateStatus(int statusId, Guid adminId, string adminComment)
        {
            if (statusId <= 0)
            {
                throw new ArgumentException("StatusId cannot be empty.", nameof(statusId));
            }
            if (adminId == Guid.Empty)
            {
                throw new ArgumentException("AdminId cannot be empty.", nameof(adminId));
            }
            StatusId = statusId;
            AdminId = adminId;
            AdminComment = adminComment ?? string.Empty;
        }
        public void UpdateTransaction(int statusId, Guid transactionId)
        {
            if (statusId <= 0)
            {
                throw new ArgumentException("StatusId cannot be empty.", nameof(statusId));
            }
            if (transactionId == Guid.Empty)
            {
                throw new ArgumentException("TransactionId cannot be empty.", nameof(transactionId));
            }
            StatusId = statusId;
            TransactionId = transactionId;
        }

    }
}
