import { Component } from '@angular/core';
import { InputTextModule } from 'primeng/inputtext';
import { CommonModule } from '@angular/common';  
import { BrowserModule } from '@angular/platform-browser';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { MessageModule } from 'primeng/message';
import { FormsModule } from '@angular/forms';
import { CheckboxModule } from 'primeng/checkbox';
import { FloatLabelModule } from 'primeng/floatlabel';
import { PasswordModule } from 'primeng/password';
@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    InputTextModule,
    CardModule,
    ButtonModule,
    MessageModule,
    FormsModule,
    CheckboxModule,
    FloatLabelModule,
    CommonModule,
    PasswordModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  public email: string = "";
  public password: string = "";
  public confirmpassword: string = "";
  public name: string = "";
  public accept: boolean = false;
  public errorMessage: string = "";
  public error=false;
  switchAccept() {
    console.log(this.accept);
    this.accept = !this.accept;
  }

  validateForm(){
    
  }
}
