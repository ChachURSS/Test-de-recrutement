import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, forkJoin } from 'rxjs';
import { map } from 'rxjs/operators';
import { Garage } from '../models/garage.model';

@Injectable({
    providedIn: 'root'
})
export class GarageService {
    private apiUrl = 'https://localhost:7068/api/garages';
    private carsApiUrl = 'https://localhost:7068/api/cars';

    constructor(private http: HttpClient) { }

    getGarages(): Observable<Garage[]> {
        return this.http.get<Garage[]>(this.apiUrl);
    }

    getGarage(id: number): Observable<Garage> {
        return this.http.get<Garage>(`${this.apiUrl}/${id}`);
    }

    getGarageById(id: number): Observable<Garage> {
        return forkJoin({
            garage: this.http.get<Garage>(`${this.apiUrl}/${id}`),
            cars: this.http.get<any[]>(`${this.carsApiUrl}?garageId=${id}`)
        }).pipe(
            map(result => {
                result.garage.cars = result.cars;
                return result.garage;
            })
        );
    }

    createGarage(garage: Garage): Observable<Garage> {
        return this.http.post<Garage>(this.apiUrl, garage);
    }

    updateGarage(id: number, garage: Garage): Observable<Garage> {
        return this.http.put<Garage>(`${this.apiUrl}/${id}`, garage);
    }

    deleteGarage(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }
}
