import { Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { LandingComponent } from './components/landing/landing.component';
import { RegisterComponent } from './components/register/register.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { HomeComponent } from './components/home/home.component';
import { ZonesComponent } from './components/zones/zones.component';
import { ConfigurationComponent } from './components/configuration/configuration.component';
import { ScheduleComponent } from './components/schedule/schedule.component';
import { ChartsComponent } from './components/charts/charts.component';
import { LiveChartsComponent } from './components/live-charts/live-charts.component';

export const routes: Routes = [
    {
        path: 'home', 
        component: HomeComponent,
        children: [
            {
                path: '',
                component: DashboardComponent,
                outlet: "home",
            },
            {
                path: 'zones',
                component: ZonesComponent,
                outlet: "home",
            },
            {
                path:'configuration',
                component:ConfigurationComponent,
                outlet: "home",
            },
            {
                path:'schedules',
                component:ScheduleComponent,
                outlet: "home",
            },
            {
                path:'charts',
                component:ChartsComponent,
                outlet: "home",
            },
            {
                path:'live-charts',
                component:LiveChartsComponent,
                outlet: "home",
            }
        ]
    },
    {
        path: '',
        component: HomeComponent,
    },
    {
        path: "register",
        component: RegisterComponent,
    },
    {
        path: "landing",
        component: LandingComponent,
    },
    {
        path: "login",
        component: LoginComponent,
    },
    // {
    //     path: '**', 
    //     component: AppComponent,
    // },
];
