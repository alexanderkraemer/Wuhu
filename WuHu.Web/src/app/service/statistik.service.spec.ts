/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { StatistikService } from './statistik.service';

describe('StatistikService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [StatistikService]
    });
  });

  it('should ...', inject([StatistikService], (service: StatistikService) => {
    expect(service).toBeTruthy();
  }));
});
