import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';

@Component({
  selector: 'app-gauge',
  imports: [],
  templateUrl: './gauge.component.html',
  styleUrl: './gauge.component.css'
})
export class GaugeComponent implements OnChanges {
  @Input() moistureLevel: number = 81;
  
  // SVG parameters
  size: number = 200;
  strokeWidth: number = 16;
  radius: number = 0;
  circumference: number = 0;
  dashoffset: number = 0;
  
  // Colors
  @Input() filledColor: string = "#FF5252";
  @Input() emptyColor: string = "#FFCDD2";
  @Input() gaugeLabel: string = "Moisture Level";

  constructor() {
    this.calculateCircleProperties();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['moistureLevel']) {
      this.calculateCircleProperties();
    }
  }

  private calculateCircleProperties(): void {
    this.radius = (this.size - this.strokeWidth) / 2;
    this.circumference = 2 * Math.PI * this.radius;
    this.dashoffset = this.circumference * (1 - this.moistureLevel / 100);
  }

  // For the demo slider
  updateMoistureLevel(event: Event): void {
    const value = parseInt((event.target as HTMLInputElement).value);
    this.moistureLevel = Math.min(100, Math.max(0, value || 0));
    this.calculateCircleProperties();
  }
}
