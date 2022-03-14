import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlanMobileComponent } from './plan-mobile.component';

describe('PlanMobileComponent', () => {
  let component: PlanMobileComponent;
  let fixture: ComponentFixture<PlanMobileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlanMobileComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlanMobileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
