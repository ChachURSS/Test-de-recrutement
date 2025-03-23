import { Component, OnInit } from '@angular/core';
import { CarService } from '../../services/car.service';
import { Car } from '../../models/car.model';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-car-list',
  templateUrl: './car-list.component.html',
  styleUrls: ['./car-list.component.scss'],
  imports: [CommonModule]
})
export class CarListComponent implements OnInit {
  cars: Car[] = [];

  constructor(
    private carService: CarService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadCars();
  }

  loadCars(): void {
    this.carService.getCars().subscribe(cars => {
      this.cars = cars;
    });
  }

    deleteCar(id: number): void {
        if (confirm('Etes vous sur de vouloir supprimer ce véhicule?')) {
            this.carService.deleteCar(id).subscribe(() => {
                this.loadCars();
            });
        }
    }

  updateCar(id: number): void {
    this.router.navigate(['car-edit', id]);
  }
}
