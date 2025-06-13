import { Injectable } from '@angular/core';
import { SensorData } from '../models/sensor-data';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private readonly WS_URL = 'https://localhost:7133/liveDataHub';
  private static connection: HubConnection | null = null; // Store the connection statically/shared
  hubConnection: HubConnection;
  selectedZoneId: string | null = null;

  constructor() {
    if (SignalrService.connection) {
      this.hubConnection = SignalrService.connection;
    } else {
      this.hubConnection = new HubConnectionBuilder()
        .withUrl(this.WS_URL)
        .build();
      SignalrService.connection = this.hubConnection;
    }
  }

  getHubConnection(): HubConnection {
    return this.hubConnection;
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

  async sendLiveDataToZone(data: SensorData): Promise<void> {
    console.log('Sending live data to zone...');
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
