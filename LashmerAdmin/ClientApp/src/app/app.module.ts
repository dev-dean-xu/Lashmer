import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { LayoutComponent } from './layout/layout.component';
import { FooterComponent } from './footer/footer.component';

import { EmployeesModule } from './employees/employees.module';
import { OrdersModule } from './orders/orders.module';
import { AccountModule } from './account/account.module';
import { ConfigService } from './shared/utils/config.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LayoutComponent,
	  FooterComponent
  ],
  imports: [
    BrowserModule,
	  AppRoutingModule,
	  FormsModule,
	  ReactiveFormsModule,
	  HttpClientModule,
	  EmployeesModule,
	  OrdersModule,
	  AccountModule,
	  NgbModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
