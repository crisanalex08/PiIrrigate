import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SensorData } from '../models/sensor-data';

@Injectable({
  providedIn: 'root'
})
export class ChartDataService {
  private apiUrl = 'https://localhost:7133/data'; // Adjust base URL as needed

  constructor(private http: HttpClient) { }

  getLineChartData(zoneId: string): Observable<SensorData[]> {
    return this.http.get<SensorData[]>(`${this.apiUrl}/${zoneId}/getalldata`);
  }
}
