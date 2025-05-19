import { Injectable } from '@angular/core';
import { AuthService } from './auth.service';
import { RegisteredUser } from '../models/registered-user';
import { Device, Zone } from '../models/zone';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ZonesService {

  private baseUrl = 'https://localhost:7133/api';
  userDetails: RegisteredUser | null = null;
  zones: Zone[] = [];
  devices: Device[] = [];
  constructor(private authService: AuthService, private http: HttpClient) {
    this.userDetails = this.authService.getUserDetails();
  }

  updateZones() {
    this.getZones().subscribe(
      (response: Zone[]) => {
        this.zones = response;
      })
  }
  public getZones() {
    return this.http.get<Zone[]>(`${this.baseUrl}/${this.userDetails?.userId}/zones`);
  }

  public getDevices(zoneId: string) {
    return this.http.get<Device[]>(`${this.baseUrl}/${zoneId}/devices`);
  }

  getZoneData(zoneId: string) {
    return this.http.get<any>(`${this.baseUrl}/${zoneId}/getAllData`);
  }

  getDeviceData(zoneId: string, deviceId: string) {
    return this.http.get<any>(`${this.baseUrl}/${zoneId}/${deviceId}/getAllData`);
  }
}
