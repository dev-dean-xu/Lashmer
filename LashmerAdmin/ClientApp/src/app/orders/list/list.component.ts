import { Component, OnInit, PipeTransform } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { Observable } from 'rxjs';
import { OrderService } from 'src/app/shared/services/order.service';
import { Order } from 'src/app/shared/models/order';
import { DecimalPipe } from '@angular/common';
import { startWith, map } from 'rxjs/operators';

function search(order: Order, term: string, pipe: PipeTransform) {
	return order.customerName.toLowerCase().includes(term) ||
		order.email.toLowerCase().includes(term) ||
		order.phoneNumber.includes(term) ||
		order.orderId.includes(term) ||
		pipe.transform(order.total).includes(term) ||
		pipe.transform(order.shipping).includes(term);
}

@Component({
	selector: 'app-list',
	templateUrl: './list.component.html',
	styleUrls: ['./list.component.css'],
	providers: [DecimalPipe]
})
export class ListComponent implements OnInit {
	private orders: Order[];
	filteredOrders: Observable<Order[]>;
	filterControl = new FormControl('');
	searchTerm: string;

	constructor(private orderService: OrderService, private pipe: DecimalPipe) { }

	ngOnInit() {
		this.getOrders();
	}

	getOrders() {
		this.orderService.getOrders().subscribe(orders => {
			this.orders = orders;

			this.filteredOrders = this.filterControl.valueChanges.pipe(
				startWith(''),
				map(text => {
					if (!text) return this.orders;
					text = text.toLowerCase();
					return this.orders.filter(order => search(order, text, this.pipe));
				})
			);
		});
	}
}
