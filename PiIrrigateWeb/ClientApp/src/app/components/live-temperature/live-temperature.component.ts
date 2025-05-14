import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
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

  constructor(private telemetryService: SignalrService) {
    console.log('TelemetryService instance:', this.telemetryService);
  }

  ngOnInit(): void {
    this.telemetryService.connect()
      .then(() => {
        console.log('Connected to SignalR hub');
        this.telemetryService.getHubConnection().on('SendLiveDataToZone', (data: any) => {
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
