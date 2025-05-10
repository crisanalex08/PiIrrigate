import { Component } from '@angular/core';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { LandingComponent } from '../landing/landing.component';
import { RouterLink, RouterOutlet } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';

@Component({
    selector: 'app-home',
    imports: [MatSlideToggleModule,
        LandingComponent,
        RouterOutlet,
        MatToolbarModule, MatSidenavModule,
        MatIconModule, MatButtonModule,
        MatMenuModule, RouterLink, MatButtonModule],
    template: `
    <mat-toolbar color="primary">
    <button type="button" mat-icon-button (click)="drawer.toggle()">
        <mat-icon>menu</mat-icon>
    </button>
    <img src="../../assets/logo.png" height="20">
    <span>PiIrrigate</span>

    <span class="spacer"></span> <button mat-icon-button matTooltip="System Status">
        <mat-icon [style.color]="systemStatusColor">circle</mat-icon> </button>

    <button mat-icon-button matTooltip="Notifications" routerLink="/alerts">
        <mat-icon matBadge="3" matBadgeColor="warn">notifications</mat-icon> </button>

    <button mat-icon-button [matMenuTriggerFor]="userMenu" matTooltip="Account">
        <mat-icon>account_circle</mat-icon>
    </button>
    <mat-menu #userMenu="matMenu">
        <button mat-menu-item routerLink="/profile">Profile</button>
        <button mat-menu-item routerLink="/settings">Settings</button>
        <button mat-menu-item (click)="logout()">Logout</button>
    </mat-menu>
</mat-toolbar>
<mat-drawer-container class="example-container" autosize>
    <mat-drawer #drawer class="example-sidenav" mode="side">
        <div class="button-container">
            <button mat-button routerLink="dashboard">Dashboard</button>
            <button mat-button routerLink="/zones">Zones</button>
            <button mat-button routerLink="/schedules">Schedules</button>
            <button mat-button routerLink="/history">History</button>
        </div>
    </mat-drawer>

    <div class="example-sidenav-content">
        <router-outlet name="home"></router-outlet>
    </div>

</mat-drawer-container>
  `,
    styles: `.button-container {
    display: flex;
    flex-direction: column;
    height: 100vh;
}

mat-drawer-container {
    height: 100vh;
}`
})
export class HomeComponent {
    title = 'ClientApp';
    systemStatusColor = 'red';


    logout() { }
}
