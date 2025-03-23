import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Car } from '../models/car.model';

@Injectable({
  providedIn: 'root'
})
export class CarService {
  private apiUrl = 'https://localhost:7068/api/cars';

  constructor(private http: HttpClient) {}

  getCars(): Observable<Car[]> {
    return this.http.get<Car[]>(this.apiUrl);
    }

    getCarsWithPagination(page: number): Observable<any> {
        return this.http.get<Car[]>(`${this.apiUrl}?page=${page}&pageSize=10`, { observe: 'response' }).pipe(
            map(response => {
                return {
                    data: response.body,
                    totalPages: +(response.headers.get('X-Total-Count') ?? 1) / 10
                };
            })
        );
    }

  getCarById(id: number): Observable<Car> {
    return this.http.get<Car>(`${this.apiUrl}/${id}`);
  }

    addCar(car: Car): Observable<Car> {
        return this.http.post<Car>(this.apiUrl, car).pipe(
            catchError(error => {
                if (error.status === 400) {
                    return throwError(() => new Error('Missing values'));
                } else if (error.status === 500) {
                    return throwError(() => new Error('Server error'));
                } else {
                    return throwError(() => new Error('Unknown error'));
                }
            })
        );
    }

  updateCar(id: number, car: Car): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, car);
  }

  deleteCar(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
