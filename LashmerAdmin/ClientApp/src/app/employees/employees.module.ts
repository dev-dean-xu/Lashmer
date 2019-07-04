import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule} from '@angular/forms'
import { ListComponent } from './list/list.component';
import { employeesRouting } from './employees.routing';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NewEmployeeComponent } from './new-employee/new-employee.component';
import { EqualValidatorDirective } from '../shared/directives/equal-validator.directive';
import { EmployeeDetailsComponent } from './employee-details/employee-details.component';
import { EmployeePromotionCodesComponent } from './employee-promotion-codes/employee-promotion-codes.component';

@NgModule({
	declarations: [ListComponent, NewEmployeeComponent, EqualValidatorDirective, EmployeeDetailsComponent, EmployeePromotionCodesComponent],
  imports: [
	  CommonModule,
	  employeesRouting,
	  FormsModule,
	  ReactiveFormsModule,
	  NgbModule
  ]
})
export class EmployeesModule { }
