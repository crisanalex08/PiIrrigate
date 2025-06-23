import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Configuration } from '../models/configuration';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConfigurationService {
  private apiUrl = 'https://localhost:7133/api/configuration'; // Adjust as needed

  constructor(private http: HttpClient) {}

  getConfiguration(): Observable<Configuration> {
    return this.http.get<Configuration>(this.apiUrl);
  }

  setConfiguration(config: Configuration): Observable<any> {
    return this.http.post(this.apiUrl, config);
  }
}
