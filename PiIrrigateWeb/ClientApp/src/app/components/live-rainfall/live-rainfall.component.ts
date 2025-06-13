import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { SensorData } from '../../models/sensor-data';
import { Subject } from 'rxjs';
import { SignalrService } from '../../services/signalr.service';
import { GaugeComponent } from '../../widgets/gauge/gauge.component';

@Component({
  selector: 'app-live-rainfall',
  standalone: true,
  imports: [GaugeComponent],
  template: `
    <app-gauge 
        [value]="telemetryData?.rainfall || 0" 
        [filledColor]="'#2196F3'" 
        [emptyColor]="'#BBDEFB'" 
        [gaugeLabel]="'Rainfall (mm)'" 
        [unit]="'mm'"
        ></app-gauge>
  `,
  styles: ``
})
export class LiveRainfallComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  telemetryData: SensorData | null = null;

  @Input() groupId!: string; // Pass the group ID (e.g., zoneId) from parent

  constructor(private telemetryService: SignalrService) {}

  ngOnInit(): void {
    this.telemetryService.joinGroup(this.groupId)
      .then(() => {
        this.telemetryService.getHubConnection().on('ReceiveLiveData', (data: SensorData) => {
          this.telemetryData = data;
          console.log('Received rainfall data:', this.telemetryData);
        });
      })
      .catch(err => console.error('Error joining group:', err));
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
