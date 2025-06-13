import { Component, inject } from '@angular/core';
import { GaugeComponent } from '../../widgets/gauge/gauge.component';
import { WidgetComponent } from '../widget/widget.component';
import { DashboardService } from '../../services/dashboard.service';
import { ZonesService } from '../../services/zones.service';
import { SignalrService } from '../../services/signalr.service'; // <-- Import SignalrService
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    MatFormFieldModule,
    MatSelectModule,
    FormsModule,
    WidgetComponent
  ],
  templateUrl: './dashboard.component.html',
  providers: [DashboardService, ZonesService, SignalrService],
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {
  store = inject(DashboardService);

  zones: any[] = [];
  selectedZoneId: string | null = null;

  constructor(
    private zonesService: ZonesService,
    private signalrService: SignalrService
  ) {
    this.zonesService.getZones().subscribe(
      (zones: any[]) => {
        this.zones = zones;
        if (zones.length > 0) {
          this.selectedZoneId = zones[0].zoneId || zones[0].id;
          this.signalrService.joinGroup(this.selectedZoneId || "");
        }
        console.log('Group joined:', this.zones);
      }
    );
  }

  onZoneChange() {
    console.log('Selected zone:', this.selectedZoneId);
    if (this.selectedZoneId) {
      this.signalrService.joinGroup(this.selectedZoneId);
    }
  }
}
