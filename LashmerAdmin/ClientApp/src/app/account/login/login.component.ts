import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router'
import { Credentials } from 'src/app/shared/models/credentials';
import { AccountService } from 'src/app/shared/services/account.service';

@Component({
	selector: 'app-login',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
	isRequesting: boolean;
	submitted: boolean = false;
	errors: string;
	credentials: Credentials = { email: '', password: '' };

	constructor(private accountService: AccountService, private router: Router) {}

	ngOnInit() {
	}

	login({ value, valid }: { value: Credentials, valid: boolean }) {
		this.submitted = true;
		this.isRequesting = true;
		this.errors = "";
		if (valid) {
			this.accountService.login(value.email, value.password).subscribe(result => {
					if (result) {
						this.router.navigate(['/']);
					}
			},
				error => this.errors = JSON.stringify(error.error)).add(() => this.isRequesting = false);
		}
	}
}
