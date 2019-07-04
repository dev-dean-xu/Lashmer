import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeePromotionCodesComponent } from './employee-promotion-codes.component';

describe('EmployeePromotionCodesComponent', () => {
  let component: EmployeePromotionCodesComponent;
  let fixture: ComponentFixture<EmployeePromotionCodesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeePromotionCodesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeePromotionCodesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
