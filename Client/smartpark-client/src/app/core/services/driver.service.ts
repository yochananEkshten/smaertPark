import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Driver, DriverInput } from '../models/driver.model';

@Injectable({ providedIn: 'root' })
export class DriverService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/Driver`;

  getAll(): Observable<Driver[]> {
    return this.http.get<Driver[]>(this.baseUrl);
  }

  getById(id: number): Observable<Driver> {
    return this.http.get<Driver>(`${this.baseUrl}/${id}`);
  }

  getByLicensePlate(licensePlate: string): Observable<Driver> {
    return this.http.get<Driver>(`${this.baseUrl}/plate/${encodeURIComponent(licensePlate)}`);
  }

  add(driver: DriverInput): Observable<number> {
    return this.http.post<number>(this.baseUrl, driver);
  }

  update(driver: Driver): Observable<void> {
    return this.http.put<void>(this.baseUrl, driver);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
