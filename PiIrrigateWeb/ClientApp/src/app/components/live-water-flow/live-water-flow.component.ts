import { Component, OnDestroy, OnInit } from '@angular/core';
import { GaugeComponent } from '../../widgets/gauge/gauge.component';
import { Subject, takeUntil } from 'rxjs';
import { SensorData } from '../../models/sensor-data';
import { DataService } from '../../services/data.service';

@Component({
  selector: 'app-live-water-flow',
  standalone: true,
  imports: [GaugeComponent],
  template: `
      <app-gauge 
        [value]="telemetryData?.waterFlow || 0" 
        [filledColor]="'#FF5252'"
        [emptyColor]="'#FFCDD2'"
        [gaugeLabel]="'Water Flow'"
        [unit]="'l/min'"
        ></app-gauge>
  `,
  styles: ``
})
export class LiveWaterFlowComponent implements OnInit, OnDestroy {
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
