import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { Schedule, ScheduleStatus } from '../../models/schedule';
import { ScheduleService } from '../../services/schedule.service';
import { ScheduleDialogComponent } from '../schedule-dialog/schedule-dialog.component';

@Component({
  selector: 'app-schedule',
  standalone: true,
  imports: [
    MatDialogModule,
    MatTableModule
  ],
  template:`
  <div>
  <h2>Schedules</h2>
  <button mat-raised-button color="primary" (click)="openCreateDialog()">Create Schedule</button>
  <table mat-table [dataSource]="schedules" class="mat-elevation-z8" style="width:100%; margin-top:16px;">
    <ng-container matColumnDef="startTime">
      <th mat-header-cell *matHeaderCellDef>Start Time</th>
      <td mat-cell *matCellDef="let s">{{ s.startTime }}</td>
    </ng-container>
    <ng-container matColumnDef="endTime">
      <th mat-header-cell *matHeaderCellDef>End Time</th>
      <td mat-cell *matCellDef="let s">{{ s.endTime }}</td>
    </ng-container>
    <ng-container matColumnDef="intervalHours">
      <th mat-header-cell *matHeaderCellDef>Interval (h)</th>
      <td mat-cell *matCellDef="let s">{{ s.intervalHours }}</td>
    </ng-container>
    <ng-container matColumnDef="duratioin">
      <th mat-header-cell *matHeaderCellDef>Duration (min)</th>
      <td mat-cell *matCellDef="let s">{{ s.duratioin }}</td>
    </ng-container>
    <ng-container matColumnDef="status">
      <th mat-header-cell *matHeaderCellDef>Status</th>
      <td mat-cell *matCellDef="let s">{{ s.status }}</td>
    </ng-container>
    <tr mat-header-row *matHeaderRowDef="['startTime','endTime','intervalHours','duratioin','status']"></tr>
    <tr mat-row *matRowDef="let row; columns: ['startTime','endTime','intervalHours','duratioin','status'];"></tr>
  </table>
</div>`,
styles:`
`
})
export class ScheduleComponent implements OnInit {
  schedules: Schedule[] = [];

  constructor(
    private scheduleService: ScheduleService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadSchedules();
  }

  loadSchedules() {
    this.scheduleService.getSchedules().subscribe(schedules => {
      this.schedules = schedules;
    });
  }

  openCreateDialog() {
    const dialogRef = this.dialog.open(ScheduleDialogComponent, {
      width: '400px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadSchedules();
      }
    });
  }
}