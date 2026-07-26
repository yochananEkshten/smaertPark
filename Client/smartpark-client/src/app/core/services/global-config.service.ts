import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { GlobalConfig } from '../models/global-config.model';

@Injectable({ providedIn: 'root' })
export class GlobalConfigService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/GlobalConfig`;

  get(): Observable<GlobalConfig> {
    return this.http.get<GlobalConfig>(this.baseUrl);
  }

  update(config: GlobalConfig): Observable<void> {
    return this.http.put<void>(this.baseUrl, config);
  }
}
