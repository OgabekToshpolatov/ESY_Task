import { Route, Router } from '@angular/router';
import { ApiService } from './../../services/api.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import ValidateForm from 'src/app/helpers/validationforms';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {

  public signUpForm!:FormGroup;
  type: string = 'password';
  isText: boolean = false;
  eyeIcon:string = "fa-eye-slash"
  eyeIcon1:string ="fa-eye-slash"
  constructor(private fb:FormBuilder, private api:ApiService, private router:Router){}

  ngOnInit(): void {
    this.signUpForm = this.fb.group({
      firstName:['', Validators.required],
      lastName:['', Validators.required],
      userName:['', Validators.required],
      email:['', Validators.required],
      password:['', Validators.required],
      confirmPassword:['',Validators.required]
    })

  }

  hideShowPass(){
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = 'fa-eye' : this.eyeIcon = 'fa-eye-slash'
    this.isText ? this.type = 'text' : this.type = 'password'
  }

  onSignUp(){
    if(this.signUpForm.valid){
      this.api.signup(this.signUpForm.value)
          .subscribe({
            next:(res) => {
              alert(res.message)
              this.signUpForm.reset();
              this.router.navigate(['signin'])
            },
            error:(err) =>{
              alert(err?.error.message)
            }
          })
    }
    else {
      ValidateForm.validateAllFormFields(this.signUpForm); //{7}
    }
  }
}
