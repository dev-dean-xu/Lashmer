import { Component, OnInit } from '@angular/core';
import {AccountService} from "../shared/services/account.service";

@Component({
	selector: 'app-nav-menu',
	templateUrl: './nav-menu.component.html',
	styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
	navbarOpen = false;
	userName = '';
	isAdmin = false;

	constructor(private accountService: AccountService) {
		this.getUserInfo();
	}

	ngOnInit() {
	}

	toggleNavbar() {
		this.navbarOpen = !this.navbarOpen;
	}

	logout() {
		this.accountService.logout();
	}

	getUserInfo() {
		let user = this.accountService.loggedUser();
		this.userName = user.user_name;
		this.isAdmin = user.user_role === 'Admin';
	}

}
