export interface CreateScheduleRequest {
    startTime: string; // ISO 8601 format
    endTime: string; // ISO 8601 format
    intervalHours: number; // Interval in hours
}

export interface Schedule{
    id:string,
    startTime: string; // ISO 8601 format
    endTime: string; // ISO 8601 format
    intervalHours: number; // Interval in hours
    duratioin: number; // Duration in minutes
}

export enum ScheduleStatus {
    NOT_STARTED = 'NOT_STARTED',
    RUNNING = 'RUNNING',
    PAUSED = 'PAUSED',
    COMPLETED = 'COMPLETED',
    FAILED = 'FAILED',
}