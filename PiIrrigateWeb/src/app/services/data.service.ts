import { Injectable } from '@angular/core';
import { DataModel } from '../common/models/data.model';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  constructor() { }

  getMockData() : DataModel[] {
    //create an array of mock data
    let mockData: DataModel[] = [];
    for (let i = 0; i < 100; i++) {
      mockData.push({
        IrrigationZoneId: i,
        SensorId: i,
        Timestamp: new Date(),
        Value: {
          Humidity: Math.random() * 100,
          Temperature: Math.random() * 100,
          SoilMoisture: Math.random() * 100,
          Rainfall: Math.random() * 100,
          WaterTemperature: Math.random() * 100,
          WaterConsumption: Math.random() * 100
        }
      });
    }
    return mockData;
  }
}
