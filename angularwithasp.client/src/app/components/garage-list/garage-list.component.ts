import { Component, OnInit } from '@angular/core';
import { GarageService } from '../../services/garage.service';
import { Garage } from '../../models/garage.model';

@Component({
  selector: 'app-garage-list',
  templateUrl: './garage-list.component.html',
  styleUrls: ['./garage-list.component.scss']
})
export class GarageListComponent implements OnInit {
  garages: Garage[] = [];

  constructor(private garageService: GarageService) { }

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
}
