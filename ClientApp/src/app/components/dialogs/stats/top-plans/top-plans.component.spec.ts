import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TopPlansComponent } from './top-plans.component';

describe('TopPlansComponent', () => {
  let component: TopPlansComponent;
  let fixture: ComponentFixture<TopPlansComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TopPlansComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TopPlansComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
