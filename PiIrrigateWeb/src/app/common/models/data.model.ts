export interface DataModel {
    IrrigationZoneId: number;
    SensorId: number;
    Timestamp: Date;
    Value: ValueModel;
}

export interface ValueModel {
    Humidity: number;
    Temperature: number;
    SoilMoisture: number;
    Rainfall: number;
    WaterTemperature: number;
    WaterConsumption: number;
}