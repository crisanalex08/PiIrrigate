import { Component } from '@angular/core';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { LandingComponent } from './components/landing/landing.component';
import { RouterLink, RouterOutlet, Router } from '@angular/router'; // <-- FIXED IMPORT
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  imports: [ 
    MatSlideToggleModule,
    RouterOutlet,
    MatToolbarModule, MatSidenavModule, 
    MatIconModule, MatButtonModule, 
    MatMenuModule, MatButtonModule
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'ClientApp';
  systemStatusColor='red';
}
