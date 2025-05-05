import { Component } from '@angular/core';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { LandingComponent } from './components/landing/landing.component';
import { RouterLink, RouterOutlet } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
@Component({
  selector: 'app-root',
  imports: [ 
    MatSlideToggleModule,
    LandingComponent,
    RouterOutlet,
    MatToolbarModule, MatSidenavModule, 
    MatIconModule, MatButtonModule, 
    MatMenuModule, RouterLink, MatButtonModule
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'ClientApp';
  systemStatusColor='red';

  
  logout(){}
}
