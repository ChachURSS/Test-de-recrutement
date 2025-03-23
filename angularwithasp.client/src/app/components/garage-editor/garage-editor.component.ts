import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { GarageService } from '../../services/garage.service';
import { Garage } from '../../models/garage.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-garage-editor',
  standalone: false,
  templateUrl: './garage-editor.component.html',
  styleUrls: ['./garage-editor.component.css']
})
export class GarageEditorComponent implements OnInit {
  garageForm!: FormGroup;
  garageId!: number;
  errorMessage: string | null = null;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private garageService: GarageService,
    private router: Router
  ) { }

  ngOnInit(): void {
    const garageIdParam = this.route.snapshot.paramMap.get('garageId');
    this.garageId = garageIdParam ? +garageIdParam : 0;
    this.garageForm = this.fb.group({
      name: ['', Validators.required],
      location: ['', Validators.required],
      capacity: ['', Validators.required]
    });

    // Check if garageId is provided as an input parameter
    if (this.garageId) {
      this.loadGarage(this.garageId);
    } else {
      this.route.params.subscribe(params => {
        this.garageId = params['id'];
        if (this.garageId) {
          this.loadGarage(this.garageId);
        }
      });
    }
  }

  loadGarage(id: number): void {
    this.garageService.getGarageById(id).subscribe(garage => {
      if (this.garageForm) {
        this.garageForm.patchValue(garage);
      }
    });
  }

  onSubmit(): void {
    this.errorMessage = null; // Reset error message
    if (this.garageForm && this.garageForm.valid) {
      const garage: Garage = this.garageForm.value;
      if (this.garageId) {
        garage.id = this.garageId;
        this.garageService.updateGarage(this.garageId, garage).subscribe({
          next: () => this.router.navigate(['garages']),
          error: (err) => {
            this.errorMessage = 'Error updating garage: ' + err;
            console.error(this.errorMessage);
          }
        });
      } else {
        this.garageService.createGarage(garage).subscribe({
          next: () => this.router.navigate(['garages']),
          error: (err) => {
            this.errorMessage = 'Error adding garage: ' + err;
            console.error(this.errorMessage);
          }
        });
      }
    }
  }
}
