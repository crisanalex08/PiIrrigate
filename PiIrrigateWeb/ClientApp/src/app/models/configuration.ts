export interface Configuration {
  soilMoistureHighThreshold: number;
  soilMoistureLowThreshold: number;
  rainDropLowThreshold: number;
  rainDropHighThreshold: number;
  waterFlowLowThreshold: number;
  waterFlowHighThreshold: number;
  waterTemperatureHighThreshold: number;
  waterTemperatureLowThreshold: number;
  irrigationRunning: boolean;
  enableNotifications: boolean;
  enableAutomation: boolean;
  refreshInterval: number;
}