import { Component, inject } from '@angular/core';
import { GaugeComponent } from '../../widgets/gauge/gauge.component';
import { WidgetComponent } from '../widget/widget.component';
import { DashboardService } from '../../services/dashboard.service';

@Component({
  selector: 'app-dashboard',
  imports: [GaugeComponent, WidgetComponent],
  templateUrl: './dashboard.component.html',
  providers: [DashboardService],
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {
  store = inject(DashboardService);
}
