import { Component } from '@angular/core';
import { Subject } from 'rxjs';
import { SignalrService } from '../../services/signalr.service';
import { GaugeComponent } from '../../widgets/gauge/gauge.component';
import { SensorData } from '../../models/sensor-data';

@Component({
  selector: 'app-live-humidity',
  imports: [GaugeComponent],
  template: `
    <app-gauge 
        [value]="telemetryData?.humidity || 0" 
        [filledColor]="'#FF5252'" 
        [emptyColor]="'#FFCDD2'" 
        [gaugeLabel]="'Humidity (°C)'" 
        ></app-gauge>
  `,
  styles: ``
})
export class LiveHumidityComponent {
private destroy$ = new Subject<void>();
  telemetryData: SensorData | null = null;

  constructor(private telemetryService: SignalrService) {
    console.log('TelemetryService instance:', this.telemetryService);
  }

  ngOnInit(): void {
    this.telemetryService.connect()
      .then(() => {
        console.log('Connected to SignalR hub');
        this.telemetryService.getHubConnection().on('SendLiveDataToZone', (data: SensorData) => {
          console.log('Received data:', data);
          this.telemetryData = data;
        });
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
