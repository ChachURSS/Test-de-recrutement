import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GarageService } from '../../services/garage.service';
import { Garage } from '../../models/garage.model';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css'],
    imports: [CommonModule]
})
export class HomeComponent implements OnInit {
    garages: Garage[] = [];
    selectedGarage: Garage | undefined;

    constructor(private garageService: GarageService, private router: Router) { }

    ngOnInit(): void {
        this.garageService.getGarages().subscribe((data: Garage[]) => {
            this.garages = data;
        });
    }

    goToGarageList(garageId: number): void {
        this.router.navigate(['/garage-list', garageId]);
    }

    selectGarage(garageId: number): void {
        this.garageService.getGarageById(garageId).subscribe((garage: Garage) => {
            this.selectedGarage = garage;
        });
    }

    redirectToNewCarScreen() {
        this.router.navigate(['/car-editgarage/', this.selectedGarage?.id]);
    }
}
