import { Component } from '@angular/core';
import { Subject } from 'rxjs';
import { SensorData } from '../../models/sensor-data';
import { SignalrService } from '../../services/signalr.service';
import { GaugeComponent } from '../../widgets/gauge/gauge.component';

@Component({
  selector: 'app-live-soil-moisture',
  imports: [GaugeComponent],
  template: `
   <app-gauge 
        [value]="telemetryData?.soilMoisture || 0" 
        [filledColor]="'#FF5252'" 
        [emptyColor]="'#FFCDD2'" 
        [gaugeLabel]="'Soil moisture'" 
        ></app-gauge>
  `,
  styles: ``
})
export class LiveSoilMoistureComponent {
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
