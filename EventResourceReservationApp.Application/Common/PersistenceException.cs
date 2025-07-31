namespace EventResourceReservationApp.Application.Common
{
    public class PersistenceException : Exception
    {
        public PersistenceException() { }
        public PersistenceException(string massage) : base(massage) { }
        public PersistenceException(string message, Exception innerException) : base(message, innerException) { }
    }
}
