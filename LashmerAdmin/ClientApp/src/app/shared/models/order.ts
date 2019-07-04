import { OrderItem } from './order-item';

export interface Order {
	orderId: string;
	customerName: string;
	phoneNumber: string;
	email: string;
	shipping: string;
	total: number;
	payment: string;
	fulfillment: string;
	items: OrderItem[];
}
