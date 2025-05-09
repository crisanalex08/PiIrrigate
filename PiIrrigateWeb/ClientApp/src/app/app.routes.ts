import { Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { LandingComponent } from './components/landing/landing.component';
import { RegisterComponent } from './components/register/register.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { HomeComponent } from './components/home/home.component';

export const routes: Routes = [
    {
        path: 'home', 
        component: HomeComponent,
        children: [
            {
                path: "dashboard",
                component: DashboardComponent,
                outlet: "home",
            },
        ]
    },
    {
        path: "login",
        component: LoginComponent,
    },
    {
        path: "register",
        component: RegisterComponent,
    },
    {
        path: "landing",
        component: LandingComponent,
    },
    // {
    //     path: '**', 
    //     component: AppComponent,
    // },
];
