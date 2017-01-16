/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { TournierComponent } from './tournier.component';

describe('TournierComponent', () => {
  let component: TournierComponent;
  let fixture: ComponentFixture<TournierComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TournierComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TournierComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
