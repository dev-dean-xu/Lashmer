import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import {DropzoneComponent, DropzoneDirective,DropzoneConfigInterface} from 'ngx-dropzone-wrapper';
import { ConfigService } from 'src/app/shared/utils/config.service';

@Component({
	selector: 'app-upload',
	templateUrl: './upload.component.html',
	styleUrls: ['./upload.component.css']
})
export class UploadComponent implements OnInit {
	config: DropzoneConfigInterface = {
		clickable: true,
		maxFiles: 1,
		autoReset: null,
		errorReset: 8000,
		cancelReset: null
	};

	constructor(private configService: ConfigService, private router: Router) {
		//let headers = new HttpHeaders();
		let authToken = localStorage.getItem('auth_token');
		this.config.headers = { 'Authorization': `Bearer ${authToken}` };
		this.config.url = configService.getApiUrl() + '/orders/upload';
	}

	ngOnInit() {
	}

	onUploadInit(args: any): void {
		console.log('onUploadInit:', args);
	}

	onUploadError(args: any): void {
		console.log('onUploadError:', args);
	}

	onUploadSuccess(args: any): void {
		console.log('onUploadSuccess:', args);
		setTimeout(() => { this.router.navigate(['/orders']); }, 2000);
	}
}
