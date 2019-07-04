import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { AccountService } from './shared/services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
	loginStatus: boolean;
	subscription: Subscription;

	constructor(private accountService: AccountService) {
			
	}
	ngOnInit(): void {
		this.subscription = this.accountService.authNavStatus$.subscribe(status => this.loginStatus = status);
	}

	ngOnDestroy(): void {
		this.subscription.unsubscribe();
	}
	


}
