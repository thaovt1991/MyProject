
import * as CryptoJS from 'crypto-js';
import { Component, ViewChild,
  ViewEncapsulation,
  OnInit,
  OnChanges,
  SimpleChanges,
  ChangeDetectorRef,
} from '@angular/core';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit{
  decrypted =''
  request=''
  responce=''
  tokenFromUI: string = "0123456789123456";
  encrypted: any = "";
   constructor(
    private changeDef : ChangeDetectorRef
   ){

   }
   
  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }

   user ={
    userName :'',
    password :'',
    confirmPass:''
   }

   isFormLogin = true;



  encryptUsingAES256() {
    let _key = CryptoJS.enc.Utf8.parse(this.tokenFromUI);
    let _iv = CryptoJS.enc.Utf8.parse(this.tokenFromUI);
    let encrypted = CryptoJS.AES.encrypt(
      JSON.stringify(this.request), _key, {
        keySize: 16,
        iv: _iv,
        mode: CryptoJS.mode.ECB,
        padding: CryptoJS.pad.Pkcs7
      });
    this.encrypted = encrypted.toString();
  }
  decryptUsingAES256() {
    let _key = CryptoJS.enc.Utf8.parse(this.tokenFromUI);
    let _iv = CryptoJS.enc.Utf8.parse(this.tokenFromUI);

    this.decrypted = CryptoJS.AES.decrypt(
      this.encrypted, _key, {
        keySize: 16,
        iv: _iv,
        mode: CryptoJS.mode.ECB,
        padding: CryptoJS.pad.Pkcs7
      }).toString(CryptoJS.enc.Utf8);
  }

  loginAccount(e ){
    debugger
  }

  openForm(e: string){
   this.isFormLogin = e== 'logIn' ;
   this.changeDef.detectChanges();
  }
}
