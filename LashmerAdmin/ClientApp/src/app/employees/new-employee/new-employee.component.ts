import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router'
import { EmployeeService } from 'src/app/shared/services/employee.service';
import { AccountService } from 'src/app/shared/services/account.service';
import { Registration } from 'src/app/shared/models/registration';

@Component({
	selector: 'app-new-employee',
	templateUrl: './new-employee.component.html',
	styleUrls: ['./new-employee.component.css']
})
export class NewEmployeeComponent implements OnInit {
	roles: string[];
	newEmployee: Registration = new Registration;
	isRequesting: boolean = false;
	errors: string;

	constructor(private employeeService: EmployeeService, private accountService: AccountService, private router: Router) { }

	ngOnInit() {
		this.getRoles();
	}

	getRoles() {
		this.accountService.getRoles().subscribe(roles => this.roles = roles);
	}

	onSubmit() {
		this.isRequesting = true;
		this.accountService.register(this.newEmployee).subscribe(() => {
			this.isRequesting = false;
			this.router.navigate(['/employees']);
		},
		error => {
			this.errors = error.error;
			this.isRequesting = false;
		});
	}
}
