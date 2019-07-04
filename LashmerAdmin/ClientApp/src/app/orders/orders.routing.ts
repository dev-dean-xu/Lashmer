import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ListComponent } from './list/list.component';
import { UploadComponent } from './upload/upload.component';
import { OrderDetailsComponent } from './order-details/order-details.component';

const routes: Routes = [
	{ path: 'orders', component: ListComponent },
	{ path: 'orders/upload', component: UploadComponent },
	{ path: 'orders/details/:id', component: OrderDetailsComponent }
];

export const ordersRouting: ModuleWithProviders = RouterModule.forChild(routes);
