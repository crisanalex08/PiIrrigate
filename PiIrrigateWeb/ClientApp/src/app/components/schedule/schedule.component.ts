import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { Schedule, ScheduleStatus } from '../../models/schedule';
import { ScheduleService } from '../../services/schedule.service';
import { ScheduleDialogComponent } from '../schedule-dialog/schedule-dialog.component';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { UpdateScheduleComponent } from '../update-schedule/update-schedule.component';

@Component({
  selector: 'app-schedule',
  standalone: true,
  imports: [
    MatDialogModule,
    MatTableModule,
    MatButtonModule,
    MatMenuModule,
    MatIconModule
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
      <td mat-cell *matCellDef="let s">{{ s.interval }}</td>
    </ng-container>
    <ng-container matColumnDef="duratioin">
      <th mat-header-cell *matHeaderCellDef>Duration (min)</th>
      <td mat-cell *matCellDef="let s">{{ s.duration }}</td>
    </ng-container>
    <ng-container matColumnDef="scheduleStatus">
      <th mat-header-cell *matHeaderCellDef>Status</th>
      <td mat-cell *matCellDef="let s">{{ getStatusName(s.scheduleStatus) }}</td>
    </ng-container>
    <ng-container matColumnDef="actions">
  <th mat-header-cell *matHeaderCellDef>Actions</th>
  <td mat-cell *matCellDef="let s">
    <button mat-icon-button [matMenuTriggerFor]="menu">
      <mat-icon>more_vert</mat-icon>
    </button>
    <mat-menu #menu="matMenu">
      <button mat-menu-item (click)="openUpdateDialog(s.id)">Edit</button>
      <button mat-menu-item (click)="deleteSchedule(s.id)">Delete</button>
    </mat-menu>
  </td>
</ng-container>
<tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
<tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
  </table>
</div>`,
styles:`
`
})
export class ScheduleComponent implements OnInit {
  schedules: Schedule[] = [];
  statusMap = ScheduleStatus;
  constructor(
    private scheduleService: ScheduleService,
    private dialog: MatDialog
  ) {}

  displayedColumns = [
  'startTime',
  'endTime',
  'intervalHours',
  'duratioin',
  'scheduleStatus',
  'actions'
];

getStatusName(status: number): string {
    switch (status) {
      case 0:
        return 'Not Started';
      case 1:
        return 'Running';
      case 2:
        return 'Paused';
      case 3:
        return 'Completed';
      case 4:
        return 'Failed';
      default:
        return 'Unknown Status';
    }
}

  ngOnInit(): void {
    this.loadSchedules();
  }

  deleteSchedule(scheduleId: string) {
    this.scheduleService.deleteSchedule(scheduleId).subscribe(() => {
      this.schedules = this.schedules.filter(s => s.id !== scheduleId);
      console.log('Schedule deleted successfully');
    }, error => {
      console.error('Error deleting schedule:', error);
    });
  }

  loadSchedules() {
    this.scheduleService.getSchedules().subscribe(schedules => {
      this.schedules = schedules;
    });
  }
  
  openUpdateDialog(scheduleId: string) {
    this.scheduleService.getScheduleById(scheduleId).subscribe(schedule => {
      const dialogRef = this.dialog.open(UpdateScheduleComponent, {
        position: { top: '20px' },
        width: '400px',
        data: schedule
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result) {
          console.log('Schedule updated:', result);
          this.scheduleService.updateSchedule(scheduleId, result).subscribe(() => {
            this.loadSchedules();
          }, error => {
            console.error('Error updating schedule:', error);
          });
        }
      });
    }, error => {
      console.error('Error fetching schedule for update:', error);
    });
  }

  openCreateDialog() {
    const dialogRef = this.dialog.open(ScheduleDialogComponent, {
      position: { top: '20px' },
      width: '400px',
    });
    

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        console.log('Schedule created:', result);
        this.schedules.push(result);
        this.scheduleService.createSchedule(result).subscribe(() => {        
          this.loadSchedules();
        }, error => {
          console.error('Error creating schedule:', error);
        });
          
      }
    });
  }
}