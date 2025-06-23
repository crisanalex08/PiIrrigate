export interface CreateScheduleRequest {
    startTime: string; // ISO 8601 format
    duration: number; // Duration in minutes
    interval: number; // Interval in hours
    endTime: string; // ISO 8601 format
    deviceId: string; // Device ID to associate with the schedule
    zoneId: string; // Timezone ID for the schedule
}

export interface Schedule{
    id:string,
    startTime: string; // ISO 8601 format
    endTime: string; // ISO 8601 format
    intervalHours: number; // Interval in hours
    duratioin: number; // Duration in minutes
    scheduleStatus: ScheduleStatus; // Status of the schedule
}

export enum ScheduleStatus {
    NOT_STARTED = 'NOT_STARTED',
    RUNNING = 'RUNNING',
    PAUSED = 'PAUSED',
    COMPLETED = 'COMPLETED',
    FAILED = 'FAILED',
}