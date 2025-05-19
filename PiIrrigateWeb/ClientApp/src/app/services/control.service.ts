import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Device } from '../models/zone';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ControlService {

  private baseUrl = 'https://localhost:7133';
  devices: Device[] = [];
  constructor(
    private httpClient: HttpClient,
    private authService: AuthService 
  ) {
    this.getDevices().subscribe(
      (devices: Device[]) => {
        this.devices = devices;
      },
      (error) => {
        console.error('Error fetching devices:', error);
      }
    );
  }

  public getDevices() {
    const userId = this.authService.getUserDetails()?.userId;
    if (!userId) {
      throw new Error('User ID not found');
    }
    return this.httpClient.get<Device[]>(`${this.baseUrl}/api/user/${userId}/devices`);
  }
}
