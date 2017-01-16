/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { SpielerService } from './spieler.service';

describe('SpielerService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SpielerService]
    });
  });

  it('should ...', inject([SpielerService], (service: SpielerService) => {
    expect(service).toBeTruthy();
  }));
});
