import { Component } from '@angular/core';
import { LineComponent } from '../../charts/line/line.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { FormsModule } from '@angular/forms';
import { ZonesService } from '../../services/zones.service';
import { ZoneData } from '../../models/zone';
import { CommonModule } from '@angular/common';
import { ChartDataService } from '../../services/chart-data.service';
import { SensorData } from '../../models/sensor-data';

@Component({
  selector: 'app-charts',
  imports: [LineComponent,
    MatFormFieldModule,
    MatSelectModule,
    FormsModule,
    CommonModule,
    LineComponent
  ],
  templateUrl: './charts.component.html',
  styleUrl: './charts.component.css'
})
export class ChartsComponent {
  selectedZoneId: string=''; // Default zone ID
  zones: ZoneData[] = []; // Array to hold zone data
  constructor(private zonesService: ZonesService, 
    private chartDataService: ChartDataService) { }

    sensorData: SensorData[] = []; // Array to hold sensor data
    
  ngOnInit() {
    this.zonesService.getZones().subscribe(
      (data) => {
        this.zones = data;
      });
  }


  onZoneChange() {
    this.chartDataService.getLineChartData(this.selectedZoneId).subscribe(
      (data) => {
        this.sensorData = data;
      },
      (error) => {
        console.error('Error fetching sensor data:', error);
      }
    );
  }

}
