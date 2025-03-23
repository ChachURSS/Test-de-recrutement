import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CarListComponent } from './components/car-list/car-list.component';
import { GarageListComponent } from './components/garage-list/garage-list.component';
import { CarEditorComponent } from './components/car-editor/car-editor.component';
import { GarageEditorComponent } from './components/garage-editor/garage-editor.component';
import { HomeComponent } from './components/home/home.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'cars', component: CarListComponent },
  { path: 'garages', component: GarageListComponent },
  { path: 'car-edit', component: CarEditorComponent },
  { path: 'car-edit/:carId', component: CarEditorComponent },
  { path: 'garage-edit', component: GarageEditorComponent },
  { path: 'garage-edit/:garageId', component: GarageEditorComponent },
  { path: '', redirectTo: '/garages', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
