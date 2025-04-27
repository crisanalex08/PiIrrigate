import { Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatIconModule } from '@angular/material/icon';
@Component({
  selector: 'app-landing',
  imports: [MatButtonModule,MatIconModule,
    MatGridListModule
  ],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.css'
})
export class LandingComponent {

  onGetStarted() { }
}
