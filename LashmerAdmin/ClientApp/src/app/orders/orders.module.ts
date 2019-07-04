import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ListComponent } from './list/list.component';
import { ordersRouting } from './orders.routing';
import { UploadComponent } from './upload/upload.component'
import {DropzoneModule, DropzoneConfigInterface,DROPZONE_CONFIG} from 'ngx-dropzone-wrapper';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { OrderDetailsComponent } from './order-details/order-details.component';

const DEFAULT_DROPZONE_CONFIG: DropzoneConfigInterface = {
	acceptedFiles: '.csv,application/vnd.ms-excel',
	createImageThumbnails: true
};


@NgModule({
	declarations: [ListComponent, UploadComponent, OrderDetailsComponent],
	imports: [
		CommonModule,
		ordersRouting,
		DropzoneModule,
		FormsModule,
		ReactiveFormsModule,
		NgbModule
	],
	providers: [
		{
			provide: DROPZONE_CONFIG,
			useValue: DEFAULT_DROPZONE_CONFIG
		}
	]
})
export class OrdersModule { }
