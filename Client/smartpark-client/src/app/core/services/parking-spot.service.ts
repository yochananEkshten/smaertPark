import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ParkingSpot } from '../models/parking.model';

@Injectable({ providedIn: 'root' })
export class ParkingSpotService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/ParkingSpot`;

  getAll(): Observable<ParkingSpot[]> {
    return this.http.get<ParkingSpot[]>(this.baseUrl);
  }

  getAvailable(): Observable<ParkingSpot[]> {
    return this.http.get<ParkingSpot[]>(`${this.baseUrl}/available`);
  }

  getByFloor(floor: number): Observable<ParkingSpot[]> {
    return this.http.get<ParkingSpot[]>(`${this.baseUrl}/floor/${floor}`);
  }

  getById(id: number): Observable<ParkingSpot> {
    return this.http.get<ParkingSpot>(`${this.baseUrl}/${id}`);
  }
}
