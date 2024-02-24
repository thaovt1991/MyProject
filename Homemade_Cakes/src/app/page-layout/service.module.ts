import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageLayoutComponent } from './page-layout.component';
import { SlickCarouselModule } from 'ngx-slick-carousel';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    PageLayoutComponent
  ],
  imports: [
    CommonModule,
    SlickCarouselModule,
    CommonModule,
    FormsModule,
  ]
})
export class ServiceModule { }
