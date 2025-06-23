import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { CreateScheduleRequest } from '../../models/schedule';

@Component({
  selector: 'app-update-schedule',
  imports: [MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatDatepickerModule,
    MatNativeDateModule,
    ReactiveFormsModule,
    MatSelectModule,
    FormsModule, CommonModule],
  templateUrl: './update-schedule.component.html',
  styleUrl: './update-schedule.component.css'
})
export class UpdateScheduleComponent { 
  form: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<UpdateScheduleComponent>,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.form = this.fb.group({
      dateRange: this.fb.group({
        start: [data?.startTime || '', Validators.required],
        end: [data?.endTime || '', Validators.required]
      }),
      interval: [data?.interval || 1, [Validators.required, Validators.min(1)]],
      duration: [data?.duration || 10, [Validators.required, Validators.min(1), Validators.max(60)]]
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
        deviceId: this.form.value.device
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
