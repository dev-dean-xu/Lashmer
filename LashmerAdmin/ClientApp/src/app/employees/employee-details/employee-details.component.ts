import { Component, OnInit } from '@angular/core';
import { Employee } from 'src/app/shared/models/employee';
import { EmployeeService } from 'src/app/shared/services/employee.service';
import { ActivatedRoute } from '@angular/router';
import { AccountService } from 'src/app/shared/services/account.service';

@Component({
	selector: 'app-employee-details',
	templateUrl: './employee-details.component.html',
	styleUrls: ['./employee-details.component.css']
})
export class EmployeeDetailsComponent implements OnInit {
	employee: Employee = new Employee;
	roles: string[];
	successMessage: string;
	errors: string[];

	constructor(private employeeService: EmployeeService, private accountService: AccountService, private route: ActivatedRoute) { }

	ngOnInit() {
		this.getRoles();
		this.getEmployee();
	}

	getEmployee() {
		const id = this.route.snapshot.paramMap.get('id');
		this.employeeService.getEmployee(id).subscribe(emp => this.employee = emp);
	}

	getRoles() {
		this.accountService.getRoles().subscribe(roles => this.roles = roles);
	}

	getPromotionCodes() {
		this.employeeService.getEmployeePromotionCodes(this.employee.id).subscribe(codes => this.employee.coupons = codes.join(","));
	}

	onSubmit() {
		this.successMessage = null;
		this.errors = null;

		this.employeeService.updateEmployee(this.employee).subscribe(
			() => this.successMessage = "The employee information has been updated successfully.",
			error => this.errors = error.error
		);
	}

	onPromotionCodesChanged() {
		this.getPromotionCodes();
	}
}
