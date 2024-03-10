import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-page-layout',
  templateUrl: './page-layout.component.html',
  styleUrls: ['./page-layout.component.css'],
})
export class PageLayoutComponent implements OnInit {
  slides = [
    { img: '../assets/images/testcakes.jpg' },
    { img: '../assets/images/banh1.jpg' },
    { img: '../assets/images/banh1.jpg' },
    { img: '../assets/images/banh3.jpg' },
    { img: '../assets/images/banh4.jpg' },
    { img: '../assets/images/banh5.jpg' },
    { img: '../assets/images/banh6.jpg' },
  ];
  slideConfig = {
    slidesToShow: 4,
    slidesToScroll: 4,
    autoplay: true,
    autoplaySpeed: 5000,
    pauseOnHover: true,
    infinite : true ,
    responsive :
    [
      {
        breakpoint :992,
        settings :{
          arrows: true,
          infinite : true ,
          slidesToShow : 3 ,
          slidesToScoll : 3 ,
        }
      },
      {
        breakpoint :768,
        settings :{
          arrows: true,
          infinite : true ,
          slidesToShow : 1 ,
          slidesToScoll : 1 ,
        }
      }
    ]
  };

  isLogIn = true

  constructor(
    private router : Router 
  ) {}
  addSlide() {
    this.slides.push({ img: 'http://placehold.it/350x150/777777' });
  }
  removeSlide() {
    this.slides.length = this.slides.length - 1;
  }
  slickInit(e: any) {
    console.log('slick initialized');
  }
  breakpoint(e: any) {
    console.log('breakpoint');
  }
  afterChange(e: any) {
    console.log('afterChange');
  }
  beforeChange(e: any) {
    console.log('beforeChange');
  }

  ngOnInit(): void {}

  click(e :any){
    this.router.navigate(['/', 'login'])
    // this.router.navigateByUrl('/loginsssss').then(nav => {
    //   console.log(nav); // true if navigation is successful
    // }, err => {
    //   console.log(err) // when there's an error
    //   this.router.navigate(['/', 'login'])
    // });;
  }
}
