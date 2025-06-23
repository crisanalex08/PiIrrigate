import { ChangeDetectionStrategy, Component } from '@angular/core';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CreateScheduleRequest } from '../../models/schedule';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { ZonesService } from '../../services/zones.service';
import { CommonModule } from '@angular/common';
import { Device, ZoneData } from '../../models/zone';

@Component({
  selector: 'app-schedule-dialog',
  imports: [MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
    ReactiveFormsModule,
    MatSelectModule,
    FormsModule, CommonModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [provideNativeDateAdapter()],
  template: `
  <h2 mat-dialog-title>Create Schedule</h2>
  <form [formGroup]="form" (ngSubmit)="onSave()">
    <mat-dialog-content style="display: flex; flex-direction: column; gap: 16px;">
    <mat-form-field>
      <mat-label>Zone</mat-label>
      <mat-select formControlName="zone" required (selectionChange)="onZoneChange()">
        <mat-option *ngFor="let zone of zonesService.zones" [value]="zone.zoneId">{{ zone.name }}</mat-option>
      </mat-select>
    </mat-form-field>
    <mat-form-field>
      <mat-label>Device</mat-label>
      <mat-select formControlName="device" required>
        <mat-option *ngFor="let device of devices" [value]="device.mac">{{ device.name }}</mat-option>
      </mat-select>
    </mat-form-field>
    <mat-form-field>
    <mat-label>Enter a date range</mat-label>
    <mat-date-range-input [rangePicker]="rangePicker" formGroupName="dateRange">
      <input matStartDate  formControlName="start" placeholder="Start date" />
      <input matEndDate formControlName="end" placeholder="End date" />
    </mat-date-range-input>
    <mat-hint>MM/DD/YYYY – MM/DD/YYYY</mat-hint>
    <mat-datepicker-toggle matIconSuffix [for]="rangePicker"></mat-datepicker-toggle>
    <mat-date-range-picker #rangePicker>
      <mat-date-range-picker-actions>
        <button matButton matDateRangePickerCancel>Cancel</button>
        <button matButton="elevated" matDateRangePickerApply>Apply</button>
      </mat-date-range-picker-actions>
    </mat-date-range-picker>
  </mat-form-field>
  
    <mat-form-field>
      <mat-label>Interval (hours)</mat-label>
      <input matInput formControlName="interval" type="number" min="1" max="24"required>
    </mat-form-field>
    <mat-form-field>
      <mat-label>Irrigaiton duration (min)</mat-label>
      <input matInput formControlName="duration" type="number" min="1" max="60" required>
    </mat-form-field>
  </mat-dialog-content>
  <mat-dialog-actions>
    <button mat-button type="button" (click)="onCancel()">Cancel</button>
    <button mat-raised-button color="primary" type="submit" [disabled]="form.invalid" cdkFocusInitial>Create</button>
  </mat-dialog-actions>
  </form>
  `,
  styles: ``
})
export class ScheduleDialogComponent {

  form: FormGroup;
  devices: Device[] = [];
  constructor(
    public dialogRef: MatDialogRef<ScheduleDialogComponent>,
    private fb: FormBuilder,
    public zonesService: ZonesService
  ) {
    zonesService.updateZones();
    this.form = this.fb.group({
      zone: ['', Validators.required],
      device: ['', Validators.required],
      dateRange: this.fb.group({
        start: ['', Validators.required],
        end: ['', Validators.required]
      }),
      interval: [1, [Validators.required, Validators.min(1)]],
      duration: [10, [Validators.required, Validators.min(1), Validators.max(60)]]
    });
  }

  onZoneChange() {
    this.zonesService.getDevicesForZone(this.form.value.zone).subscribe(devices => {
      this.form.patchValue({ device: '' }); // Reset device selection
      this.devices = devices;
      console.log('Devices for selected zone:', this.devices);
    });
  }

  onSave() {
    if (this.form.invalid) {
      console.warn('Form is invalid:', this.form.errors, this.form.value);
      Object.keys(this.form.controls).forEach(key => {
        const control = this.form.get(key);
        if (control && control.invalid) {
          console.warn(`Control "${key}" is invalid:`, control.errors, control.value);
        }
      });
      return;
    }
    if (this.form.valid) {
      // Combine date and time into ISO strings
      const start = this.combineDateAndTime(this.form.value.dateRange.start);
      const end = this.combineDateAndTime(this.form.value.dateRange.end);
      const req: CreateScheduleRequest = {
        startTime: start,
        duration: this.form.value.duration,
        interval: this.form.value.interval,
        endTime: end,
        zoneId: this.form.value.zone,
        deviceId: this.form.value.device // Assuming you have a deviceId in the form
      };

      console.log('Form is valid, submitting:', req);

      this.dialogRef.close(req);
    }
  }

  onCancel() {
    this.dialogRef.close();
  }

  private combineDateAndTime(date: string): string {
    const d = new Date(date);
    const time = d.toTimeString().split(' ')[0]; // Get time in HH:MM:SS format
    const [hours, minutes, seconds] = time.split(':').map(Number);
    d.setHours(hours, minutes, 0, 0);
    return d.toISOString();
  }
}
