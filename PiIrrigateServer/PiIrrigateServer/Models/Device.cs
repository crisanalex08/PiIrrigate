using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PiIrrigateServer.Models
{
    public class Device
    {
        [Key]
        public string Mac { get; set; } // Unique identifier for the device and primary key  
        public Guid ZoneId { get; set; } // Unique identifier for the zone  
        public string? Name { get; set; } // Name of the device  
        public string? Location { get; set; } // Location of the device  
        public string? Owner { get; set; } // Owner of the device  
        public string? Description { get; set; } // Description of the device  
        public bool IsRegistered { get; set; } // Registration status  
        public Zone Zone { get; set; } = null!; // Navigation property to the Zone entity  
    }
}
