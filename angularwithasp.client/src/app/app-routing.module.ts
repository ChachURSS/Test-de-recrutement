import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GarageListComponent } from './components/garage-list/garage-list.component';
import { CarListComponent } from './components/car-list/car-list.component';

const routes: Routes = [
  { path: '', redirectTo: '/garages', pathMatch: 'full' },
  { path: 'garages', component: GarageListComponent },
  { path: 'cars', component: CarListComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
