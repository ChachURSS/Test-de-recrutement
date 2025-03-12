import { Component, OnInit } from '@angular/core';
import { CarService } from '../../services/car.service';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-car-list',
  templateUrl: './car-list.component.html',
  styleUrls: ['./car-list.component.scss']
})
export class CarListComponent implements OnInit {
  cars: any[] = [];
  carForm: FormGroup;

  constructor(private carService: CarService, private fb: FormBuilder) {
    this.carForm = this.fb.group({
      model: [''],
      garageId: ['']
    });
  }

  ngOnInit(): void {
    this.loadCars();
  }

  loadCars(): void {
    this.carService.getCars().subscribe(data => {
      this.cars = data;
    });
  }

  addCar(): void {
    this.carService.addCar(this.carForm.value).subscribe(() => {
      this.loadCars();
    });
  }
}
