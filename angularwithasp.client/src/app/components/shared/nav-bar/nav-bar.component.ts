import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-bar',
  standalone: false,
  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.css'
})
export class NavBarComponent {
  constructor(private router: Router) { }

  goToGarageList(): void {
    this.router.navigate(['/', 'garages']);
  }

  goToCarList(): void {
    this.router.navigate(['/', 'cars']);
  }

  goToHome(): void {
    this.router.navigate(['/', 'home']);
  }

  goToAddGarage(): void {
    this.router.navigate(['/', 'garage-edit']);
  }

  goToAddCar(): void {
    this.router.navigate(['/', 'car-edit']);
  }

}
