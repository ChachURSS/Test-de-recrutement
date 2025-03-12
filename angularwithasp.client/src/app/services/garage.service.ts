import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class GarageService {
  private apiUrl = 'https://api.example.com/garages';

  constructor(private http: HttpClient) { }

  getGarages(): Observable<any> {
    return this.http.get(this.apiUrl);
  }

  addGarage(garage: any): Observable<any> {
    return this.http.post(this.apiUrl, garage);
  }

  deleteGarage(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
