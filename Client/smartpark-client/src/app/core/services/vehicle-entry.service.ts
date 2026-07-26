import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { VehicleEntry, VehicleEntryRequest } from '../models/parking.model';

@Injectable({ providedIn: 'root' })
export class VehicleEntryService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/VehicleEntry`;

  getAll(): Observable<VehicleEntry[]> {
    return this.http.get<VehicleEntry[]>(this.baseUrl);
  }

  getActive(): Observable<VehicleEntry[]> {
    return this.http.get<VehicleEntry[]>(`${this.baseUrl}/active`);
  }

  getById(id: number): Observable<VehicleEntry> {
    return this.http.get<VehicleEntry>(`${this.baseUrl}/${id}`);
  }

  getActiveByLicensePlate(licensePlate: string): Observable<VehicleEntry> {
    return this.http.get<VehicleEntry>(`${this.baseUrl}/plate/${encodeURIComponent(licensePlate)}`);
  }

  entry(request: VehicleEntryRequest): Observable<number> {
    return this.http.post<number>(`${this.baseUrl}/entry`, request);
  }

  exit(id: number): Observable<number> {
    return this.http.post<number>(`${this.baseUrl}/exit/${id}`, {});
  }
}
