import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Meter {
  id: string;
  serialNumber: string;
  type: 'Electricity' | 'Water' | 'Gas';
  status: 'Active' | 'Inactive' | 'Faulty';
  siteName: string;
}

export interface Alert {
  id: string;
  meterId: string;
  severity: 'Info' | 'Warning' | 'Critical';
  message: string;
  acknowledged: boolean;
  createdAt: string;
}

@Injectable({ providedIn: 'root' })
export class ApiService {
  private readonly base = '/api';

  constructor(private http: HttpClient) {}

  getMeters(): Observable<Meter[]> {
    return this.http.get<Meter[]>(`${this.base}/meters`);
  }

  getAlerts(onlyUnacknowledged = false): Observable<Alert[]> {
    return this.http.get<Alert[]>(`${this.base}/alerts?onlyUnacknowledged=${onlyUnacknowledged}`);
  }

  acknowledgeAlert(id: string): Observable<void> {
    return this.http.post<void>(`${this.base}/alerts/${id}/acknowledge`, {});
  }
}
