import { Component, OnInit } from '@angular/core';
import { GarageService } from '../../services/garage.service';
import { Garage } from '../../models/garage.model';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-garage-list',
  templateUrl: './garage-list.component.html',
  styleUrls: ['./garage-list.component.scss'],
  imports: [CommonModule]
})
export class GarageListComponent implements OnInit {
  garages: Garage[] = [];

  constructor(
    private garageService: GarageService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadGarages();
  }

  loadGarages(): void {
    this.garageService.getGarages().subscribe(garages => {
      this.garages = garages;
    });
  }

  deleteGarage(id: number): void {
    this.garageService.deleteGarage(id).subscribe(() => {
      this.loadGarages();
    });
  }

  updateGarage(id: number): void {
    this.router.navigate(['garage-edit', id]);
  }
}
