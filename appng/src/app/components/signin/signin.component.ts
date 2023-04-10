import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import ValidateForm from 'src/app/helpers/validationforms';
import { AuthService } from 'src/app/services/auth.service';
import { UserStoreService } from 'src/app/services/user-store.service';

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
    private auth:AuthService,
    private router:Router,
    private userStore:UserStoreService){}

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

  onSignIn(){
    if(this.signInForm.valid){
      this.auth.signin(this.signInForm.value)
          .subscribe({
            next:(res) => {
              alert("Succesfully boldi okalar ")
              this.signInForm.reset();
              this.auth.storeToken(res.token)
              let tokenPayload = this.auth.decodedToken();
              this.userStore.setRoleForStore(tokenPayload.role);
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
