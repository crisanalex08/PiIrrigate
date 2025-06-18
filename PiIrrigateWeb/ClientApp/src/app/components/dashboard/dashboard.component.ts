import { Component, inject } from '@angular/core';
import { DashboardService } from '../../services/dashboard.service';
import { ZonesService } from '../../services/zones.service';
import { SignalrService } from '../../services/signalr.service';
import { DataService } from '../../services/data.service'; // <-- Import DataService
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { FormsModule } from '@angular/forms';
import { WidgetComponent } from '../widget/widget.component';

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
    private signalrService: SignalrService,
    private dataService: DataService // <-- Inject DataService
  ) {
    this.zonesService.getZones().subscribe(
      (zones: any[]) => {
        this.zones = zones;
        if (zones.length > 0) {
          this.selectedZoneId = zones[0].zoneId || zones[0].id;
          this.handleZoneChange(this.selectedZoneId || "");
        }
      }
    );
  }

  onZoneChange() {
    if (this.selectedZoneId) {
      this.handleZoneChange(this.selectedZoneId);
    }
  }

  private handleZoneChange(zoneId: string) {
    this.signalrService.joinGroup(zoneId);
    // Set the selected zone in DashboardService
    this.store.selectedZoneId.set(zoneId);
    // Fetch and store initial/live data in DataService
    this.dataService.getInitialAndLiveData(zoneId).subscribe();
  }
}
