import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { DataService } from '../../services/data.service';
import { GaugeComponent } from '../../widgets/gauge/gauge.component';
import { SensorData } from '../../models/sensor-data';

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
