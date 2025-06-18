import { Injectable } from '@angular/core';
import { SensorData } from '../models/sensor-data';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private readonly WS_URL = 'https://localhost:7133/liveDataHub';
  private static connection: HubConnection | null = null; // Store the connection statically/shared
  hubConnection: HubConnection;
  selectedZoneId: string | null = null;

  constructor(private http: HttpClient) {
    if (SignalrService.connection) {
      this.hubConnection = SignalrService.connection;
    } else {
      this.hubConnection = new HubConnectionBuilder()
        .withUrl(this.WS_URL)
        .build();
      SignalrService.connection = this.hubConnection;
    }
  }
  async connect(): Promise<void> {
    if (this.hubConnection.state === 'Connected') {
      return;
    }
    try {
      await this.hubConnection.start();
      console.log('SignalR connection established');
    } catch (error) {
      console.error('Error establishing SignalR connection:', error);
    }
  }
  
  /**
   * Joins the SignalR group and listens for live data.
   * @param groupId The group (zone) ID
   * @param onData Callback to handle received live data
   */
  async startReceivingLiveData(groupId: string, onData: (data: SensorData) => void): Promise<void> {
    await this.joinGroup(groupId);

    // Listen for live SignalR data
    this.hubConnection.on('ReceiveLiveData', (data: SensorData) => {
      onData(data);
    });
  }

  async joinGroup(groupId: string): Promise<void> {
    try {
      await this.connect();
      await this.hubConnection.invoke('JoinGroup', groupId);
      this.selectedZoneId = groupId;
      console.log(`Joined group: ${groupId}`);
    } catch (error) {
      console.error('Error joining group:', error);
    }
  }
}
