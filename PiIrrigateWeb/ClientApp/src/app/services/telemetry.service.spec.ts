import { TestBed } from '@angular/core/testing';

import { SignalrService } from './signalr.service';

describe('TelemetryService', () => {
  let service: SignalrService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SignalrService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
