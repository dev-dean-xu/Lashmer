import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BaseService } from './base.service';
import { ConfigService } from '../utils/config.service';
import { Observable, BehaviorSubject } from 'rxjs'
import { map, tap } from 'rxjs/operators';
import { Auth_Token} from "../models/auth_token";
import { Registration } from '../models/registration';

const httpOptions = {
	headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
	providedIn: 'root'
})
export class AccountService extends BaseService {
	baseUrl: string = "";
	private loggedIn = false;
	// Observable navItem source
	private _authNavStatusSource = new BehaviorSubject<boolean>(false);
	// Observable navItem stream
	authNavStatus$ = this._authNavStatusSource.asObservable();

	constructor(private http: HttpClient, private configService: ConfigService) {
		super();
		this.baseUrl = configService.getApiUrl();
		this.loggedIn = !!localStorage.getItem('auth_token');
		this._authNavStatusSource.next(this.loggedIn);
	}

	register(registration: Registration) {
		let url = this.baseUrl + '/accounts';
		let headers = this.getHttpHeadersWithAutoToken();
		return this.http.post(url, registration, { headers }).pipe(
			tap(() => console.log('A new user registered.'))
		);
	}

	login(email, password) {
		let url = this.baseUrl + '/auth';

		return this.http.post<Auth_Token>(url, JSON.stringify({ email, password }), httpOptions).pipe(
			tap(res => {
				localStorage.setItem('auth_user_id', res.user_id);
				localStorage.setItem('auth_user_name', res.user_name);
				localStorage.setItem('auth_token', res.auth_token);
				localStorage.setItem('auth_user_role', res.user_role);
				this.loggedIn = true;
				this._authNavStatusSource.next(true);
			}),
			map(res => {
				if (res != null && res.auth_token != null) return true;
				else return false;
			})
		);
	}

	logout() {
		localStorage.removeItem('auth_token');
		localStorage.removeItem('auth_user_id');
		localStorage.removeItem('auth_user_name');
		this.loggedIn = false;
		this._authNavStatusSource.next(false);
	}

	isLoggedIn() {
		return this.loggedIn;
	}

	//loggedUserName() {
	//	return localStorage.getItem('auth_user_name');
	//}

	loggedUser() {
		return new Auth_Token(localStorage.getItem('auth_user_id'),
			localStorage.getItem('auth_user_name'),
			localStorage.getItem('auth_user_role'),
			null,
			null);
	}

	getRoles() {
		let url = this.baseUrl + '/accounts/roles';
		let headers = this.getHttpHeadersWithAutoToken();
		return this.http.get<string[]>(url, { headers });
	}
}
