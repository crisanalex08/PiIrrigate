namespace PiIrrigateServer.Models
{
    public class Configuration
    {
        public int soilMoistureHighThreshold {get;set;}
        public int soilMoistureLowThreshold { get; set;}
        public int rainDropLowThreshold {get;set;}
        public int rainDropHighThreshold {get;set;}
        public int waterFlowLowThreshold {get;set;}
        public int waterFlowHighThreshold {get;set;}
        public int waterTemperatureHighThreshold {get;set;}
        public int waterTemperatureLowThreshold {get;set;}
        public bool irrigationRunning {get;set;}
        public bool enableNotifications {get;set;}
    }
}
