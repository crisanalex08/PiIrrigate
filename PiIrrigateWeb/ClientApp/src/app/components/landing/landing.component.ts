import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';
@Component({
  selector: 'app-landing',
  imports: [MatButtonModule,MatIconModule,
    MatGridListModule, CommonModule
  ],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.css'
})
export class LandingComponent {

  features = [
    {
      icon: 'autorenew',
      iconColor: '#1976d2',
      title: 'Automation',
      description: 'Effortlessly automate your irrigation schedules and let PiIrrigate take care of watering your plants at the optimal times, saving you time and ensuring healthy growth.'
    },
    {
      icon: 'cloud',
      iconColor: '#0288d1',
      title: 'Cloud Connected',
      description: 'Access and control your irrigation system from anywhere with seamless cloud integration.'
    },
    {
      icon: 'device_hub',
      iconColor: '#fbc02d',
      title: 'Scalable',
      description: 'Easily expand and manage multiple zones or devices as your needs grow.'
    },
    {
      icon: 'touch_app',
      iconColor: '#388e3c',
      title: 'Easy to Use',
      description: 'Simple and intuitive interface designed for everyone, no technical expertise required.'
    },
    {
      icon: 'satellite_alt',
      iconColor: '#7b1fa2',
      title: 'Usable in Remote Areas',
      description: 'Reliable operation even in remote locations, ensuring your plants are always cared for.'
    }
  ];

  currentFeature = 0;

  constructor(private router: Router) {}

  prevFeature() {
    this.currentFeature =
      (this.currentFeature - 3 + this.features.length) % this.features.length;
  }

  nextFeature() {
    this.currentFeature =
      (this.currentFeature + 3) % this.features.length;
  }

  onGetStarted() {
    this.router.navigate(['/register']);
  }

  onCarouselWheel(event: WheelEvent, carousel: HTMLElement) {
    if (event.deltaY === 0) return;
    event.preventDefault();
    carousel.scrollBy({ left: event.deltaY *5, behavior: 'smooth' });
  }
}
