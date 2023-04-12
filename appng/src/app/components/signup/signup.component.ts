import { Route, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, MinLengthValidator, Validators } from '@angular/forms';
import ValidateForm from 'src/app/helpers/validationforms';
import { AuthService } from 'src/app/services/auth.service';
import { ToastrService } from 'ngx-toastr';

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
  constructor(private fb:FormBuilder, private auth:AuthService, private router:Router, private toastr:ToastrService){}

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
      const password = this.signUpForm.get('password')!.value;
      console.log(password)
      this.auth.signup(this.signUpForm.value)
          .subscribe({
            next:(res) => {
              this.toastr.success(res.message)
              this.signUpForm.reset();
              this.router.navigate(['signin'])
            },
            error:(err) =>{
              this.toastr.error(err?.error.message)
            }
          })
    }
    else {
      ValidateForm.validateAllFormFields(this.signUpForm); //{7}
    }
  }
}
