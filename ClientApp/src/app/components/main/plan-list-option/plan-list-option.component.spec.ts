import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlanListOptionComponent } from './plan-list-option.component';

describe('PlanListOptionComponent', () => {
  let component: PlanListOptionComponent;
  let fixture: ComponentFixture<PlanListOptionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlanListOptionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlanListOptionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
