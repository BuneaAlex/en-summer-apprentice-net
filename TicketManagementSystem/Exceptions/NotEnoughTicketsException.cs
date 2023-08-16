namespace TicketManagementSystem.Exceptions
{
    public class NotEnoughTicketsException : Exception
    {
        public NotEnoughTicketsException() { }
        public NotEnoughTicketsException(string message) : base(message) { }

        public NotEnoughTicketsException(string message, Exception innerException) : base(message, innerException) { }

        public NotEnoughTicketsException(int entityId, string entityType) : base(FormattableString.Invariant($"'{entityType}' with id:'{entityId}' not found!"))
        { }
    }
}
