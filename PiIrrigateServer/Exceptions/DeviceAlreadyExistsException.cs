namespace PiIrrigateServer.Exceptions
{
    [Serializable]
    internal class DeviceAlreadyExistsException : Exception
    {
        public DeviceAlreadyExistsException()
        {
        }

        public DeviceAlreadyExistsException(string? message) : base(message)
        {
        }

        public DeviceAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}