import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BaseService } from './base.service';
import { ConfigService } from '../utils/config.service';
import { Employee } from '../models/employee';
import { tap, filter } from 'rxjs/operators';



@Injectable({
  providedIn: 'root'
})
export class EmployeeService extends BaseService {
	baseUrl: string = "";
	searchTerm: string;

	constructor(private http: HttpClient, private configService: ConfigService) {
		super();
		this.baseUrl = configService.getApiUrl();
	}

	getEmployees() {
		const url = this.baseUrl + "/employees";
		const headers = this.getHttpHeadersWithAutoToken();
		return this.http.get<Employee[]>(url, { headers }).pipe(
			tap(res => console.log("Get ${res.length} employees"))
				//filter(employee => match(employee, this.searchTerm)),
				//tap(res => console.log("Get ${res.length} employees"),
			);
	}

	getEmployee(id) {
		const url = this.baseUrl + "/employees/" + id;
		const headers = this.getHttpHeadersWithAutoToken();
		return this.http.get<Employee>(url, { headers }).pipe(
			tap(res => console.log("Get employee ${res.userName}"))
		);
	}

	updateEmployee(employee: Employee) {
		const url = this.baseUrl + "/employees/" + employee.id;
		const headers = this.getHttpHeadersWithAutoToken();
		return this.http.put(url, employee, { headers }).pipe(
			tap(() => console.log("Updating an employee."))
		);
	}

	getEmployeePromotionCodes(employeeId) {
		const url = `${this.baseUrl}/employees/${employeeId}/coupons`;
		const headers = this.getHttpHeadersWithAutoToken();
		return this.http.get<string[]>(url, { headers }).pipe(
			tap(() => console.log("Getting promotion codes of an employee."))
		);
	}

	addEmployeePromotionCode(employeeId, promotionCode) {
		const url = `${this.baseUrl}/employees/${employeeId}/coupons`;
		const headers = this.getHttpHeadersWithAutoToken();
		return this.http.post(url, { UserId: employeeId, Coupon: promotionCode }, { headers }).pipe(
			tap(() => console.log("Adding a promotion code to an employee."))
		);
	}

	deleteEmployeePromotionCode(employeeId, promotionCode) {
		const url = `${this.baseUrl}/employees/${employeeId}/coupons/${promotionCode}`;
		const headers = this.getHttpHeadersWithAutoToken();
		return this.http.delete(url, { headers }).pipe(
			tap(() => console.log("Deleting a promotion code from an employee."))
		);
	}
}
