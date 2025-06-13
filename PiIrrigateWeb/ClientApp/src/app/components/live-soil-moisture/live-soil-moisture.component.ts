import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { SensorData } from '../../models/sensor-data';
import { SignalrService } from '../../services/signalr.service';
import { GaugeComponent } from '../../widgets/gauge/gauge.component';

@Component({
  selector: 'app-live-soil-moisture',
  standalone: true,
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
export class LiveSoilMoistureComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  telemetryData: SensorData | null = null;

  @Input() groupId!: string; // Pass the group ID (e.g., zoneId) from parent

  constructor(private telemetryService: SignalrService) {}

  ngOnInit(): void {
    this.telemetryService.joinGroup(this.groupId)
      .then(() => {
        this.telemetryService.getHubConnection().on('ReceiveLiveData', (data: SensorData) => {
          this.telemetryData = data;
          console.log('Received soil moisture data:', this.telemetryData);
        });
      })
      .catch(err => console.error('Error joining group:', err));
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
