namespace TicketManagementSystem.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() { }
        public EntityNotFoundException(string message) : base(message) { }

        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException) { }

        public EntityNotFoundException(int entityId,string entityType) : base(FormattableString.Invariant($"'{entityType}' with id:'{entityId}' not found!"))
        { }
    }
}
