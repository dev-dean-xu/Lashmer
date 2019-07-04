import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { LoginComponent } from './login.component';
import { FormsModule } from '@angular/forms'
import { AccountService } from 'src/app/shared/services/account.service';
import { Router } from '@angular/router';

describe('LoginComponent', () => {
  let component: LoginComponent;
	let fixture: ComponentFixture<LoginComponent>;
	let accountServiceStub: Partial<AccountService>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
		declarations: [LoginComponent],
		imports: [FormsModule],
		providers: [{ Provide: AccountService, useValue: {} }]
    })
    .compileComponents();
  }));

	beforeEach(() => {
	
    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
