import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ListComponent } from './list/list.component';
import { NewEmployeeComponent } from './new-employee/new-employee.component'
import { EmployeeDetailsComponent } from './employee-details/employee-details.component';

const routes: Routes = [
	{ path: 'employees', component: ListComponent },
	{ path: 'employees/new', component: NewEmployeeComponent },
	{ path: 'employees/details/:id', component: EmployeeDetailsComponent }
];

export const employeesRouting: ModuleWithProviders = RouterModule.forChild(routes);
