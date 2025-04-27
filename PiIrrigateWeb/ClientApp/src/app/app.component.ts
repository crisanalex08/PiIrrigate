import { Component } from '@angular/core';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { HeaderComponent } from './components/header/header.component';
import { LandingComponent } from './components/landing/landing.component';
import { RouterOutlet } from '@angular/router';
@Component({
  selector: 'app-root',
  imports: [ 
    MatSlideToggleModule,
    HeaderComponent,
    LandingComponent,
    RouterOutlet
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'ClientApp';
}
