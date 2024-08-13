import { APP_INITIALIZER, CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { PageLayoutComponent } from './page-layout/page-layout.component';
import { SlickCarouselModule } from 'ngx-slick-carousel';

import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ServiceModule } from './page-layout/service.module';
import { HttpClientModule } from '@angular/common/http';
import { LoginComponent } from './service/login/login.component';

import { RouteReuseStrategy } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthComponent } from './service/auth/auth.component';
@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    AuthComponent,
  ],
  imports: [
    HttpClientModule,
    BrowserModule,
    AppRoutingModule,
    ServiceModule,
    FormsModule ,
    BrowserAnimationsModule, //Thay đổi trạng thái mượt mà
    CommonModule,
  ],
  
  providers:[
  // {
  //   provide: APP_INITIALIZER,
  //   useFactory: APP_INITIALIZER,
  //   multi: true,
  //  // deps: [AuthService, AppConfigService],
  // },
  // { provide: RouteReuseStrategy}
  ],
  bootstrap: [AppComponent],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA,
    NO_ERRORS_SCHEMA
  ]
})
export class AppModule { }
