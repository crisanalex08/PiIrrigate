import { Component } from '@angular/core';
import { MatSelectModule } from '@angular/material/select';
import { ControlService } from '../../services/control.service';
import { CommonModule } from '@angular/common';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';

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
        style="margin: 20px; width: 200px;">
        <mat-option *ngFor="let device of devices" [value]="device.mac">
          {{ device.name }}
        </mat-option>
      </mat-select>
    </div>
    <div class="controls">
      <mat-slide-toggle
        class="toggle"
        style="margin: 20px; width: 200px;">
        Turn irrigation on/off
      </mat-slide-toggle>
      <mat-form-field style="margin: 20px; width: 200px;">
        <mat-label>Set refresh rate (minutes)</mat-label>
        <input matInput type="number" [(ngModel)]="duration" min="1" />
      </mat-form-field>
      <button mat-raised-button color="primary" (click)="submitDuration()">
        Submit
      </button>
    </div>
  `,
  styles: ``
})
export class ControlComponent {
  duration: number = 10;

  constructor(private controlService: ControlService) {}

  get devices() {
    return this.controlService.devices;
  }

  submitDuration() {
    // Implement your submit logic here
    console.log('Submitted duration:', this.duration);
  }
}
