import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { GarageListComponent } from './components/garage-list/garage-list.component';
import { CarListComponent } from './components/car-list/car-list.component';
import { GarageService } from './services/garage.service';
import { CarService } from './services/car.service';

@NgModule({
  declarations: [
    AppComponent,
    GarageListComponent,
    CarListComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [GarageService, CarService],
  bootstrap: [AppComponent]
})
export class AppModule { }
