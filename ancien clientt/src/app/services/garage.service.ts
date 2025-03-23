import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Garage } from '../models/garage.model';

@Injectable({
  providedIn: 'root'
})
export class GarageService {
  private apiUrl = 'https://localhost:5001/api/garages';

  constructor(private http: HttpClient) { }

  getGarages(): Observable<Garage[]> {
    return this.http.get<Garage[]>(this.apiUrl);
  }

  getGarage(id: number): Observable<Garage> {
    return this.http.get<Garage>(`${this.apiUrl}/${id}`);
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
