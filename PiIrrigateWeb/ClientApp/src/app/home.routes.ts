import { Routes } from '@angular/router';

import { DashboardComponent } from './components/dashboard/dashboard.component';

export const homeRoutes: Routes = [
    {
        path: '',
        component: DashboardComponent,
        outlet: 'home'
    },
];
