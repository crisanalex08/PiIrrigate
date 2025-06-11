namespace PiIrrigateServer.Models
{
    public class C2DMethodCall
    {
        public string DeviceId { get; set; }
        public string Method { get; set; }
        public MethodParams[] Params { get; set; }
    }

    public class MethodParams
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
