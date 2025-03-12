import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class CarService {
  private apiUrl = 'https://api.example.com/cars';

  constructor(private http: HttpClient) { }

  getCars(): Observable<any> {
    return this.http.get(this.apiUrl);
  }

  addCar(car: any): Observable<any> {
    return this.http.post(this.apiUrl, car);
  }

  deleteCar(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  assignCarToGarage(carId: number, garageId: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/${carId}/assign/${garageId}`, {});
  }

  removeCarFromGarage(carId: number): Observable<any> {
    return this.http.put(`${this.apiUrl}/${carId}/remove`, {});
  }
}
