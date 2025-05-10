import { Injectable } from '@angular/core';
import { SensorData } from '../models/sensor-data'; // Adjust the import path as necessary
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private readonly WS_URL = 'https://localhost:7133/liveDataHub'; // Replace with your WebSocket endpoint
  hubConnection: HubConnection;

  constructor() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.WS_URL)
      .build();
  }

  getHubConnection(): HubConnection {
    return this.hubConnection;
  }

  async connect(): Promise<void> {
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
}
