import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { CarService } from '../../services/car.service';
import { GarageService } from '../../services/garage.service';
import { Car } from '../../models/car.model';
import { Garage } from '../../models/garage.model';
import { forkJoin } from 'rxjs';
import { Router } from '@angular/router';

@Component({
    selector: 'app-car-editor',
    standalone: false,
    templateUrl: './car-editor.component.html',
    styleUrls: ['./car-editor.component.css']
})
export class CarEditorComponent implements OnInit {
    carForm!: FormGroup;
    carId!: number;
    garages!: Garage[];
    errorMessage: string | null = null;

    constructor(
        private fb: FormBuilder,
        private route: ActivatedRoute,
        private carService: CarService,
        private garageService: GarageService,
        private router: Router
    ) { }

    ngOnInit(): void {
        const carIdParam = this.route.snapshot.paramMap.get('carId');
        const garageIdParam = this.route.snapshot.paramMap.get('garageId');
        this.carId = carIdParam ? +carIdParam : 0;
        this.carForm = this.fb.group({
            brand: ['', Validators.required],
            model: ['', Validators.required],
            color: ['', Validators.required],
            garageId: [null]
        });

        // Check if carId is provided as an input parameter
        if (this.carId) {
            this.loadCar(this.carId);
        } else {
            this.route.params.subscribe(params => {
                this.carId = params['id'];
                if (this.carId) {
                    this.loadCar(this.carId);
                }
            });
        }

        this.loadGarages();

        // Check if garageId is provided as an input parameter
        if (garageIdParam) {
            // select the garage with this id in the srop down list
            this.carForm.patchValue({ garageId: garageIdParam });
            // set the garage id in the car object
            this.carForm.value.garageId = garageIdParam;
        }
    }

    loadCar(id: number): void {
        this.carService.getCarById(id).subscribe(car => {
            if (this.carForm) {
                this.carForm.patchValue(car);
            }
        });
    }

    loadGarages(): void {
        this.garageService.getGarages().subscribe(garages => {
            this.garages = garages;
        });
    }

    onSubmit(): void {
        this.errorMessage = null; // Reset error message
        if (this.carForm && this.carForm.valid) {
            const car: Car = this.carForm.value;
            if (this.carId) {
                car.id = this.carId;
                this.carService.updateCar(this.carId, car).subscribe({
                    next: () => this.router.navigate(['cars']),
                    error: (err) => {
                        this.errorMessage = 'Error updating car: ' + err;
                        console.error(this.errorMessage);
                    }
                });
            } else {
                this.carService.addCar(car).subscribe({
                    next: () => this.router.navigate(['cars']),
                    error: (err) => {
                        this.errorMessage = 'Error adding car: ' + err;
                        console.error(this.errorMessage);
                    }
                });
            }
        }
    }
}
