import { Injectable, signal } from '@angular/core';
import { Widget } from '../models/dashboard';
import { GaugeComponent } from '../widgets/gauge/gauge.component';
import { LiveTemperatureComponent } from '../components/live-temperature/live-temperature.component';

@Injectable()
export class DashboardService {

  widgets = signal<Widget[]>([
    {
      id: 0,
      label: 'Temperature',
      content: LiveTemperatureComponent
    },
    {
      id: 1,
      label: 'Temperature',
      content: GaugeComponent
    }
  ]);
  constructor() { }
}

