import { Component, OnInit } from '@angular/core';
import { GarageService } from '../../services/garage.service';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-garage-list',
  templateUrl: './garage-list.component.html',
  styleUrls: ['./garage-list.component.scss']
})
export class GarageListComponent implements OnInit {
  garages: any[] = [];
  garageForm: FormGroup;

  constructor(private garageService: GarageService, private fb: FormBuilder) {
    this.garageForm = this.fb.group({
      name: ['']
    });
  }

  ngOnInit(): void {
    this.loadGarages();
  }

  loadGarages(): void {
    this.garageService.getGarages().subscribe(data => {
      this.garages = data;
    });
  }

  addGarage(): void {
    this.garageService.addGarage(this.garageForm.value).subscribe(() => {
      this.loadGarages();
    });
  }
}
