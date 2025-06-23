import { Component, Input, OnChanges, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { SensorData } from '../../models/sensor-data';
import { Chart, LineController, LineElement, PointElement, LinearScale, Title, CategoryScale, Legend, Tooltip } from 'chart.js';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

Chart.register(LineController, LineElement, PointElement, LinearScale, Title, CategoryScale, Legend, Tooltip);

@Component({
  selector: 'app-line',
  templateUrl: './line.component.html',
  imports: [CommonModule, FormsModule],
  styleUrls: ['./line.component.css']
})
export class LineComponent implements OnChanges, AfterViewInit {
  @Input() sensorData: SensorData[] = [];

  @ViewChild('myChartCanvas') myChartCanvas!: ElementRef<HTMLCanvasElement>;

  availableFeatures: string[] = [];
  featureSelections: { [key: string]: boolean } = {};

  chart: Chart | undefined;
  private viewInitialized = false;

  ngAfterViewInit() {
    this.viewInitialized = true;
    this.tryCreateChart();
  }

  ngOnChanges() {
    if (this.sensorData && this.sensorData.length > 0) {
      this.availableFeatures = Object.keys(this.sensorData[0])
        .filter(key => !['timestamp', 'zoneId', 'mac', 'latitude', 'longitude', 'readingTime'].includes(key));
      this.availableFeatures.forEach(f => {
        if (this.featureSelections[f] === undefined) {
          this.featureSelections[f] = (f.toLowerCase() === 'soilmoisture');
        }
      });
      this.tryCreateChart();
    }
  }

  onFeatureSelectionChange() {
    this.tryCreateChart();
  }

  tryCreateChart() {
    if (this.viewInitialized && this.myChartCanvas) {
      this.createChart();
    }
  }

  createChart() {
    const labels = this.sensorData.map(d =>
      typeof d.timestamp === 'string'
        ? new Date(d.timestamp).toISOString().slice(0, 10)
        : d.timestamp.toISOString().slice(0, 10)
    );

    const datasets = this.availableFeatures
      .filter(f => this.featureSelections[f])
      .map(f => ({
        label: f,
        data: this.sensorData.map(d => (d as any)[f]),
        backgroundColor: this.getColor(f),
        borderColor: this.getColor(f),
        fill: false,
        tension: 0.1
      }));

    if (this.chart) {
      this.chart.destroy();
    }
    this.chart = new Chart(this.myChartCanvas.nativeElement, {
      type: 'line',
      data: {
        labels: labels,
        datasets: datasets
      },
      options: {
        aspectRatio: 2.5
      }
    });
  }

  getColor(feature: string): string {
    const colors = [
      'blue', 'green', 'red', 'orange', 'purple', 'lime', 'teal', 'brown', 'pink', 'gray'
    ];
    const idx = this.availableFeatures.indexOf(feature);
    return colors[idx % colors.length];
  }
}
