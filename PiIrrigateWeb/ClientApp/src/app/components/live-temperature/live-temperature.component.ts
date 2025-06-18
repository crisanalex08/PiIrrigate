import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { DataService } from '../../services/data.service';
import { GaugeComponent } from '../../widgets/gauge/gauge.component';
import { SensorData } from '../../models/sensor-data';

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
  telemetryData: SensorData | null = null;

  constructor(private dataService: DataService) {}

  ngOnInit(): void {
    this.dataService.getLatestData()
      .pipe(takeUntil(this.destroy$))
      .subscribe((data: SensorData | null) => {
        this.telemetryData = data;
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
