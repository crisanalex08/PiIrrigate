import { Injectable, signal } from '@angular/core';
import { Widget } from '../models/dashboard';
import { GaugeComponent } from '../widgets/gauge/gauge.component';
import { LiveTemperatureComponent } from '../components/live-temperature/live-temperature.component';
import { LiveHumidityComponent } from '../components/live-humidity/live-humidity.component';
import { LiveSoilMoistureComponent } from '../components/live-soil-moisture/live-soil-moisture.component';
import { LiveRainfallComponent } from '../components/live-rainfall/live-rainfall.component';
import { ControlComponent } from '../components/control/control.component';
import { LiveWaterFlowComponent } from '../components/live-water-flow/live-water-flow.component';
import { LiveWaterTemperatureComponent } from '../components/live-water-temperature/live-water-temperature.component';

@Injectable()
export class DashboardService {

  selectedZoneId = signal<string | null>(null);
  widgets = signal<Widget[]>([
    {
      id: 4,
      label: 'Control',
      content: ControlComponent
    },
    {
      id: 0,
      label: 'Temperature',
      content: LiveTemperatureComponent
    },
    {
      id: 1,
      label: 'Humidity',
      content: LiveHumidityComponent
    },
    {
      id: 2,
      label: 'Soil Moisture',
      content: LiveSoilMoistureComponent
    },
    {
      id: 3,
      label: 'Rainfall',
      content: LiveRainfallComponent
    },
    {
      id: 4,
      label: 'Water Flow',
      content: LiveWaterFlowComponent
    },
    {
      id: 5,
      label: 'Water Temperature',
      content: LiveWaterTemperatureComponent
    },    
  ]);
  constructor() { }
}

