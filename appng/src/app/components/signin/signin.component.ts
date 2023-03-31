import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import ValidateForm from 'src/app/helpers/validationforms';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css']
})
export class SigninComponent implements OnInit{

  public signInForm!:FormGroup;
  type: string = 'password';
  isText: boolean = false;
  eyeIcon: string = 'fa-eye-slash';

  constructor(
    private fb:FormBuilder,
    private api:ApiService,
    private router:Router){}

  ngOnInit(): void {
    this.signInForm = this.fb.group({
      username:['',Validators.required],
      password:['',Validators.required]
    })

  }

  hideShowPass() {
    this.isText = !this.isText;
    this.isText ? (this.eyeIcon = 'fa-eye') : (this.eyeIcon = 'fa-eye-slash');
    this.isText ? (this.type = 'text') : (this.type = 'password');
  }

  onSignUp(){
    if(this.signInForm.valid){
      this.api.signin(this.signInForm.value)
          .subscribe({
            next:(res) => {
              alert("Succesfully boldi okalar ")
              this.signInForm.reset();
              this.router.navigate(['dashboard'])
            },
            error:(err) =>{
              alert(" Rasvo boldi okalar ")
            }
          })
    }
    else {
      ValidateForm.validateAllFormFields(this.signInForm);
    }
  }

}
