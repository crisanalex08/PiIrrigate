namespace PiIrrigateServer.Exceptions
{
    [Serializable]
    internal class ZoneAlreadyExistsException : Exception
    {
        public ZoneAlreadyExistsException()
        {
        }

        public ZoneAlreadyExistsException(string? message) : base(message)
        {
        }

        public ZoneAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}