/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { SpielerComponent } from './spieler.component';

describe('SpielerComponent', () => {
  let component: SpielerComponent;
  let fixture: ComponentFixture<SpielerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SpielerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SpielerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
