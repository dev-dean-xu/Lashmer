import { Component, OnInit } from '@angular/core';
import { Observable, from, merge } from 'rxjs';
import { map, startWith } from 'rxjs/operators'
import { EmployeeService } from 'src/app/shared/services/employee.service';
import { Employee } from 'src/app/shared/models/employee';
import { FormControl } from '@angular/forms';

function match(employee: Employee, term: string) {
	return employee.fullName.toLowerCase().includes(term) ||
		employee.email.toLowerCase().includes(term) ||
		employee.userName.toLowerCase().includes(term) ||
		employee.role.toLowerCase().includes(term) ||
		employee.coupons.toLowerCase().includes(term);
}

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {
	private employees: Employee[];
	filteredEmployees: Employee[];
	filterControl = new FormControl('');

	constructor(private employeeService: EmployeeService) { }

	ngOnInit() {
		this.getEmployees();
		this.filterEmployees();
	}

	getEmployees() {
		this.employeeService.getEmployees().subscribe(employees => {
			this.employees = employees;
			this.filteredEmployees = employees;
		});
	}

	filterEmployees() {
		this.filterControl.valueChanges.pipe(
			startWith(''),
			map(text => text.toLowerCase())
		).subscribe(text => {
			if (!this.employees) return;
			if (!text) this.filteredEmployees = this.employees;
			this.filteredEmployees = this.employees.filter(employee => match(employee, text));
		});
	}
}
