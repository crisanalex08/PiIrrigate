import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { SignalrService } from '../../services/signalr.service';
import { GaugeComponent } from '../../widgets/gauge/gauge.component';
import { SensorData } from '../../models/sensor-data';

@Component({
  selector: 'app-live-humidity',
  standalone: true,
  imports: [GaugeComponent],
  template: `
    <app-gauge 
        [value]="telemetryData?.humidity || 0" 
        [filledColor]="'#2196F3'"
        [emptyColor]="'#BBDEFB'"
        [gaugeLabel]="'Humidity (%)'"
        [unit]="'%'"
        ></app-gauge>
  `,
  styles: ``
})
export class LiveHumidityComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  telemetryData: SensorData | null = null;

  @Input() groupId!: string; // Pass the group ID (e.g., zoneId) from parent

  constructor(private telemetryService: SignalrService) {}

  ngOnInit(): void {
    this.telemetryService.joinGroup(this.groupId)
      .then(() => {
        // Listen for group data
        this.telemetryService.getHubConnection().on('ReceiveLiveData', (data: SensorData) => {
          this.telemetryData = data;
          console.log('Received live data:', this.telemetryData);
        });
      })
      .catch(err => console.error('Error joining group:', err));
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
