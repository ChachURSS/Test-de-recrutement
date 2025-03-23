import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CarListComponent } from './components/car-list/car-list.component';
import { GarageListComponent } from './components/garage-list/garage-list.component';

const routes: Routes = [
  { path: 'cars', component: CarListComponent },
  { path: 'garages', component: GarageListComponent },
  { path: '', redirectTo: '/cars', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
