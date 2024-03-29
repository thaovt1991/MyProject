import { CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA, NgModule } from '@angular/core';
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

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
  ],
  imports: [
    HttpClientModule,
    BrowserModule,
    AppRoutingModule,
    ServiceModule,
    FormsModule 
  ],
  providers: [],
  bootstrap: [AppComponent],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA,
    NO_ERRORS_SCHEMA
  ]
})
export class AppModule { }
