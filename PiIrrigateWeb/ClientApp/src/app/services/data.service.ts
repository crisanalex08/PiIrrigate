import { Injectable } from '@angular/core';
import { SignalrService } from './signalr.service';
import { SensorData } from '../models/sensor-data';
import { Observable, BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  private initialDataSubject = new BehaviorSubject<SensorData | null>(null);
  initialData$ = this.initialDataSubject.asObservable();

  constructor(private signalR: SignalrService, private http: HttpClient) { }

  /**
   * Gets the initial data for a group and then starts the SignalR connection for live updates.
   * Stores the latest data for use by other components.
   * @param groupId The group (zone) ID
   */
  getInitialAndLiveData(groupId: string): Observable<SensorData> {
    return new Observable<SensorData>(observer => {
      // Fetch initial data from DataController
      this.http.get<SensorData>(`https://localhost:7133/data/${groupId}/getLastData`).subscribe({
        next: (data) => {
          console.log('Initial data fetched:', data);
          this.initialDataSubject.next(data);
          observer.next(data);
        },
        error: (err) => {
          console.error('Error fetching initial data:', err);
        }
      });

      // Listen for live SignalR data
      this.signalR.startReceivingLiveData(groupId, (data: SensorData) => {
        this.initialDataSubject.next(data);
        observer.next(data);
      });
    });
  }

  /**
   * Returns the latest stored data (as observable).
   */
  getLatestData(): Observable<SensorData | null> {
    return this.initialData$;
  }
}
