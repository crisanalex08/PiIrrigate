import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { SignalrService } from '../../services/signalr.service';
import { GaugeComponent } from '../../widgets/gauge/gauge.component';

@Component({
  selector: 'app-live-temperature',
  standalone: true,
  imports: [GaugeComponent],
  template: `
      <app-gauge 
        [value]="telemetryData?.temperature || 0" 
        [filledColor]="'#FF5252'"
        [emptyColor]="'#FFCDD2'"
        [gaugeLabel]="'Temperature (°C)'"
        [unit]="'°C'"
        ></app-gauge>
  `,
  styles: ``
})
export class LiveTemperatureComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  telemetryData: any;

  @Input() groupId!: string; // Pass the group ID (e.g., zoneId) from parent

  constructor(private telemetryService: SignalrService) {}

  ngOnInit(): void {
    this.telemetryService.connect()
      .then(() => {
        // Join the group after connecting
        this.telemetryService.getHubConnection()
          .invoke('JoinGroup', this.groupId)
          .then(() => {
            console.log(`Joined group: ${this.groupId}`);
          })
          .catch(err => console.error('Error joining group:', err));

        // Listen for group data
        this.telemetryService.getHubConnection().on('ReceiveLiveData', (data: any) => {
          this.telemetryData = data;
          console.log('Received live data:', this.telemetryData);
        });
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
