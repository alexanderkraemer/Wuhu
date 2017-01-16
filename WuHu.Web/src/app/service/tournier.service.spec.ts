/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { TournierService } from './tournier.service';

describe('TournierService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [TournierService]
    });
  });

  it('should ...', inject([TournierService], (service: TournierService) => {
    expect(service).toBeTruthy();
  }));
});
