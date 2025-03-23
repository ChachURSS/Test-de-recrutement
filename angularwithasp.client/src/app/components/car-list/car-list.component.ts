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
    currentPage: number = 1;
    totalPages: number = 1;
    totalPagesArray: number[] = [];

  constructor(
    private carService: CarService,
    private router: Router
  ) { }

  ngOnInit(): void {
      this.loadCarsWithPagination();
  }

  loadCars(): void {
    this.carService.getCars().subscribe(cars => {
      this.cars = cars;
    });
    }

    loadCarsWithPagination(): void {
        this.carService.getCarsWithPagination(this.currentPage).subscribe(response => {
            this.cars = response.data;
            this.totalPages = response.totalPages;
            //this.totalPagesArray = Array(this.totalPages).fill(0).map((x, i) => i + 1);
        });
    }

    changePage(page: number): void {
        if (page >= 1 && page <= this.totalPages) {
            this.currentPage = page;
            this.loadCarsWithPagination();
        }
    }

    deleteCar(id: number): void {
        if (confirm('Etes vous sur de vouloir supprimer ce véhicule?')) {
            this.carService.deleteCar(id).subscribe(() => {
                this.loadCarsWithPagination();
            });
        }
    }

  updateCar(id: number): void {
    this.router.navigate(['car-edit', id]);
  }
}
