import { Component } from '@angular/core';
import { SensorData } from '../../models/sensor-data';
import { Subject } from 'rxjs';
import { SignalrService } from '../../services/signalr.service';
import { GaugeComponent } from '../../widgets/gauge/gauge.component';

@Component({
  selector: 'app-live-rainfall',
  imports: [GaugeComponent],
  template: `
    <app-gauge 
        [value]="telemetryData?.humidity || 0" 
        [filledColor]="'#FF5252'" 
        [emptyColor]="'#FFCDD2'" 
        [gaugeLabel]="'Rain (°C)'" 
        ></app-gauge>
  `,
  styles: ``
})
export class LiveRainfallComponent {
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
