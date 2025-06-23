import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private apiUrl = 'https://localhost:7133/api/irrigation'; // Adjust as needed

  constructor(private http: HttpClient) {}

  startIrrigation(deviceId: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/start/${deviceId}`, { deviceId });
  }

  stopIrrigation(deviceId: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/stop/${deviceId}`, { deviceId });
  }
}
