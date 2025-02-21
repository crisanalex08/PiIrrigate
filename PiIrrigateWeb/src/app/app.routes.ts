import { Routes } from '@angular/router';
import { TableDataViewComponent } from './components/table-data-view/table-data-view.component';

export const routes: Routes = [
    {path: '**', component: TableDataViewComponent},
];
