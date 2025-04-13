import { RouterModule, Routes } from '@angular/router';
import { LandingComponent } from './components/landing/landing.component';
import { NgModule } from '@angular/core';

export const routes: Routes = [
    {path: '', component: LandingComponent},
    {path: '**', component: LandingComponent},
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
  })
  
  export class AppRoutingModule {
      
      constructor() { }
   }
  
