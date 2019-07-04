import { Injectable } from '@angular/core';

@Injectable({
	providedIn: 'root'
})
export class ConfigService {
	_apiUrl: string;

	constructor() {
		this._apiUrl = "http://localhost:5000/api";
	}

	getApiUrl() {
		return this._apiUrl;
	}
}
