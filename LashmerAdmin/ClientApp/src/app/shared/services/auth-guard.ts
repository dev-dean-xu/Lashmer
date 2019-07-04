import { Injectable } from '@angular/core';
import { AccountService } from './account.service';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

@Injectable()
export class AuthGuard implements CanActivate {
	canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
		return true;
	}
}
