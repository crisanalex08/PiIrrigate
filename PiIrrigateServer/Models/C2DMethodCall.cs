using System.ComponentModel.DataAnnotations;

namespace PiIrrigateServer.Models
{
    public class C2DMethodCall
    {
        [Key]
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public string Method { get; set; }
        public MethodParams[] Params { get; set; }
    }

    public class MethodParams
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
