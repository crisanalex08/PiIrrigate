namespace PiIrrigateServer.Exceptions
{
    [Serializable]
    internal class ZoneNotFoundException : Exception
    {
        public ZoneNotFoundException()
        {
        }

        public ZoneNotFoundException(string? message) : base(message)
        {
        }

        public ZoneNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}