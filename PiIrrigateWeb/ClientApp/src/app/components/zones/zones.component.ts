import { Component } from '@angular/core';
import { MatSelectModule } from '@angular/material/select';
import { ZonesService } from '../../services/zones.service';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { SensorReading } from '../../models/zone';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-zones',
  imports: [MatSelectModule, CommonModule, MatTableModule, FormsModule],
  providers: [ZonesService],
  template: `
    <div class="header">
      <h2 class="title">Zones</h2>
    </div>
    <div class="description">
      <p>Select a zone to view its devices and sensor data.</p>
    </div>
    <div class="dropdowns">
      <mat-select
        placeholder="Select a zone"
        class="select"
        style="margin: 20px; width: 200px;"
        [(ngModel)]="selectedZoneId"
        (selectionChange)="onZoneChange()">
        <mat-option *ngFor="let zone of zones" [value]="zone.zoneId">
          {{ zone.name }}
        </mat-option>
      </mat-select>

      <!-- Select a device -->
      <mat-select
        placeholder="Select a device"
        class="select"
        style="margin: 20px; width: 200px;"
        [(ngModel)]="selectedDeviceId"
        (selectionChange)="onDeviceChange()"
        [disabled]="!showDeviceDropdown">
        <mat-option *ngFor="let device of devices" [value]="device.mac">
          {{ device.name }}
        </mat-option>
      </mat-select>
    </div>

    <div class="content">
      <table mat-table [dataSource]="data" class="mat-elevation-z8">
        <ng-container matColumnDef="temperature">
          <th mat-header-cell *matHeaderCellDef> Temperature </th>
          <td mat-cell *matCellDef="let element"> {{element.temperature}} °C </td>
        </ng-container>
        <ng-container matColumnDef="humidity">
          <th mat-header-cell *matHeaderCellDef> Humidity </th>
          <td mat-cell *matCellDef="let element"> {{element.humidity}} % </td>
        </ng-container>
        <ng-container matColumnDef="soilMoisture">
          <th mat-header-cell *matHeaderCellDef> Soil Moisture </th>
          <td mat-cell *matCellDef="let element"> {{element.soilMoisture}} % </td>
        </ng-container>
        <ng-container matColumnDef="rainfall">
          <th mat-header-cell *matHeaderCellDef> Rainfall </th>
          <td mat-cell *matCellDef="let element"> {{element.rainfall}} mm </td>
        </ng-container>
        <ng-container matColumnDef="timestamp">
          <th mat-header-cell *matHeaderCellDef> Timestamp </th>
          <td mat-cell *matCellDef="let element"> {{element.timestamp | date:'short'}} </td>
        </ng-container>
        <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
        <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
      </table>
    </div>
  `,
  styles: `
  .description{
    margin: 20px;
  }
  .dropdowns{
    display= "flex";
  }
  `
})
export class ZonesComponent {
  selectedZoneId: string | null = null;

  showDeviceDropdown: boolean = this.selectedZoneId ? true : false;
  selectedDeviceId: string | null = null;

  public data: SensorReading[] = [];
  displayedColumns: string[] = ['temperature', 'humidity', 'soilMoisture', 'rainfall', 'timestamp'];

  constructor(private zonesService: ZonesService) {
    this.zonesService.updateZones();
  }

  get zones() {
    return this.zonesService.zones;
  }

  get devices() {
    return this.zonesService.devices;
  }

  onZoneChange() {
    if (this.selectedZoneId) {
      this.zonesService.getZoneData(this.selectedZoneId).subscribe(
        (response: SensorReading[]) => {
          this.data = response;
          this.zonesService.getDevices(this.selectedZoneId!).subscribe(
            (response: any) => {
              this.zonesService.devices = response;
            }
          );
          this.showDeviceDropdown = true;
        }
      );
    } else {
      this.data = [];
    }
  }

  onDeviceChange() {
    if (this.selectedDeviceId && this.selectedZoneId) {
      this.data.filter((reading) => reading.mac === this.selectedDeviceId);
    } 
  }
}
