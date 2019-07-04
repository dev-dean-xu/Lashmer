import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs'
import { map, tap } from 'rxjs/operators';
import { BaseService } from './base.service';
import { ConfigService } from '../utils/config.service';
import { Order } from '../models/order';

const httpOptions = {
	headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};


@Injectable({
  providedIn: 'root'
})
export class OrderService extends BaseService {
	baseUrl: string = "";
	constructor(private http: HttpClient, private configService: ConfigService) {
		super();
		this.baseUrl = configService.getApiUrl();
	}

	getOrders() {
		let url = this.baseUrl + "/orders";
		let headers = this.getHttpHeadersWithAutoToken();
		return this.http.get<Order[]>(url, { headers }).pipe(
			tap(res => console.log("Get ${res.length} orders")
		));
	}
}
