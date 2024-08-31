import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PageLayoutComponent } from './page-layout/page-layout.component';
import { LoginComponent } from './service/login/login.component';

const routes: Routes = [
  {
    path :'home' ,
    component : PageLayoutComponent ,
     pathMatch:'full'
  },
  {
    path :'login' ,
    component : LoginComponent ,
     pathMatch:'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppRoutingModule { }
