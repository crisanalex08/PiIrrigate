import { Component } from '@angular/core';
import { ButtonModule  } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { PanelModule } from 'primeng/panel';
import { CarouselModule } from 'primeng/carousel';
import { Router } from '@angular/router';
@Component({
  selector: 'app-landing',
  standalone: true,
  imports: [ButtonModule, 
    CardModule, 
    PanelModule, 
    CarouselModule],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.css'
})
export class LandingComponent {

  constructor(private router: Router){}
  public features: Feature[] = [
    {
      title: 'Real Time Monitoring',
      description: 'View live sensor data and irrigation status.',
      icon: 'pi pi-check-circle',
    },
    {
      title: 'Automation',
      description: 'Smart scheduling based on soil moisture levels.',
      icon: 'pi pi-calendar'
    },
    {
      title: 'Mobile access',
      description: 'Control your irrigation from anywhere.',
      icon: 'pi pi-tablet'
    },
  ]

  goToRegister() {
    this.router.navigate(['/register']);
  }

}


interface Feature {
  title: string;
  description: string;
  icon: string;
  height?: string;
  width?: string;
}
