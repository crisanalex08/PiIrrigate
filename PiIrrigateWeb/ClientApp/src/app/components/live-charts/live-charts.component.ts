import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, ElementRef, OnChanges, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { SignalrService } from '../../services/signalr.service';
import { Chart, LineController, LineElement, PointElement, LinearScale, Title, CategoryScale, Legend, Tooltip } from 'chart.js';
import { DataService } from '../../services/data.service';
import { ZonesService } from '../../services/zones.service';
import { Zone } from '../../models/zone';
import { SensorData } from '../../models/sensor-data';

Chart.register(LineController, LineElement, PointElement, LinearScale, Title, CategoryScale, Legend, Tooltip);


@Component({
  selector: 'app-live-charts',
  imports: [FormsModule, CommonModule, MatFormFieldModule, MatSelectModule],
  templateUrl: './live-charts.component.html',
  styleUrl: './live-charts.component.css'
})
export class LiveChartsComponent implements OnInit, OnChanges, AfterViewInit {
  selectedZoneId: string = '';
  zones: Zone[] = [];

  @ViewChild('myChartCanvas') myChartCanvas!: ElementRef<HTMLCanvasElement>;
  

  availableFeatures: string[] = [];
  featureSelections: { [key: string]: boolean } = {};
  chart: Chart | undefined;
  private viewInitialized = false;
  sensorData: SensorData[] = []; // Replace with actual type if known


  constructor(private signalRService : SignalrService, 
    public dataService: DataService,
    private zoneService: ZonesService
  ) { }


  ngOnInit(): void {
    this.zoneService.getZones().subscribe(zones => {
      this.zones = zones;
      if (zones.length > 0) {
        this.selectedZoneId = zones[0].zoneId || zones[0].zoneId;
        this.onZoneChange();
      }
    });

    if (this.selectedZoneId) {
      this.onZoneChange();
    }

    
  }

    onFeatureSelectionChange() {
    this.tryCreateChart();
  }

  ngAfterViewInit(): void {
    this.viewInitialized = true;
    this.tryCreateChart();
  }
  ngOnChanges(changes: SimpleChanges): void {

  }

  updateFeatureSelections(sensorData: SensorData[]): void {
    if (sensorData && sensorData.length > 0) {
      this.availableFeatures = Object.keys(sensorData[0])
        .filter(key => !['timestamp', 'zoneId', 'mac', 'latitude', 'longitude', 'readingTime'].includes(key));
      this.availableFeatures.forEach(f => {
        if (this.featureSelections[f] === undefined) {
          this.featureSelections[f] = (f.toLowerCase() === 'soilmoisture');
        }
      });
    }
    this.tryCreateChart();

  }

  onZoneChange(): void {
    this.signalRService.joinGroup(this.selectedZoneId);
    // Fetch and store initial/live data in DataService
    this.dataService.getInitialAndLiveData(this.selectedZoneId).subscribe(
      (data: SensorData) => {
        this.sensorData.push(data);
        this.updateFeatureSelections(this.sensorData);
        console.log('Initial data fetched:', data);
        this.tryCreateChart();
      },
      error => {
        console.error('Error fetching initial data:', error);
      }
    );
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
