import { Component } from '@angular/core';
import { MatSelectModule } from '@angular/material/select';
import { ControlService } from '../../services/control.service';
import { CommonModule } from '@angular/common';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-controll',
  imports: [
    MatSelectModule,
    MatInputModule,
    MatSlideToggleModule,
    MatButtonModule,
    CommonModule,
    FormsModule
  ],
  template: `
    <div class="description">
      <p>Control the irrigation system.</p>
    </div>
    <div class="dropdowns">
      <mat-select
        placeholder="Select a device"
        class="select"
        style="margin: 20px; width: 200px;"
        [(ngModel)]="selectedDeviceMac"
        (selectionChange)="onDeviceChange()">
        <mat-option *ngFor="let device of devices" [value]="device.mac">
          {{ device.name }}
        </mat-option>
      </mat-select>
    </div>
    <div class="controls">
      <button mat-raised-button color="green" style="margin-left: 16px;" (click)="startIrrigation()">
        Start Irrigation
      </button>
      <button mat-raised-button color="warn" style="margin-left: 8px;" (click)="stopIrrigation()">
        Stop Irrigation
      </button>
    </div>
    <div *ngIf="irrigationRunning" style="margin: 16px 0;">
      Irrigation is running
    </div>
    <div *ngIf="!irrigationRunning" style="margin: 16px 0;">
      Irrigation is stopped
    </div>
  `,
  styles: ``
})
export class ControlComponent {
  duration: number = 10;
  selectedDeviceMac: string | null = null;
  irrigationRunning: boolean = false;

  constructor(private controlService: ControlService, private apiService: ApiService ) {}

  get devices() {
    return this.controlService.devices;
  }

  onDeviceChange() {
    console.log('Selected device MAC:', this.selectedDeviceMac);
    const selectedDevice = this.devices.find(d => d.mac === this.selectedDeviceMac);
    console.log('Selected device:', selectedDevice);
  }

  submitDuration() {
    console.log('Submitted duration:', this.duration);
  }

  startIrrigation() {
    console.log('Start irrigation for device:', this.selectedDeviceMac);
    if (!this.selectedDeviceMac) {
      console.error('No device selected for irrigation.');
      return;
    }

    this.apiService.startIrrigation(this.selectedDeviceMac).subscribe(
      response => {
        console.log('Irrigation started successfully:', response);
      },
      error => {
        console.error('Error starting irrigation:', error);
      });
    this.irrigationRunning = true;
    // Add your start logic here
  }

  stopIrrigation() {
    console.log('Stop irrigation for device:', this.selectedDeviceMac);
    if (!this.selectedDeviceMac) {
      console.error('No device selected for irrigation.');
      return;
    }

    this.apiService.stopIrrigation(this.selectedDeviceMac).subscribe(
      response => {
        console.log('Irrigation stoped successfully:', response);
      },
      error => {
        console.error('Error stoped irrigation:', error);
      });
    this.irrigationRunning = false;
    // Add your stop logic here
  }
}
