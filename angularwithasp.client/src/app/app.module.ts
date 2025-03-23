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
import { HomeComponent } from './components/home/home.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NavBarComponent } from './components/shared/nav-bar/nav-bar.component';
import { CarEditorComponent } from './components/car-editor/car-editor.component';
import { GarageEditorComponent } from './components/garage-editor/garage-editor.component';

@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    CarEditorComponent,
    GarageEditorComponent
  ],
  imports: [
    BrowserModule,
    NgbModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    GarageListComponent,
    CarListComponent,
    HomeComponent
  ],
  providers: [GarageService, CarService],
  bootstrap: [AppComponent]
})
export class AppModule { }
