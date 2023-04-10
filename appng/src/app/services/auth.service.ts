import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import {JwtHelperService} from '@auth0/angular-jwt'

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrlAccount : string = "https://localhost:7048/api/Account/"
  private userPayload : any;
  constructor(private http:HttpClient,private router:Router) {
    this.userPayload = this.decodedToken()
  }

  signup(signUp:any){
    return this.http.post<any>(`${this.baseUrlAccount}signup`,signUp)
  }

  signin(signIn:any){
    return this.http.post<any>(`${this.baseUrlAccount}signin`,signIn)
  }

  storeToken(tokenValue:string){
    localStorage.setItem('token',tokenValue);
  }

  signOut(){
    localStorage.clear();
    this.router.navigate(['signin']);
  }

  getToken(){
    return localStorage.getItem('token')
  }

  isLoggedIn():boolean{
    return !!localStorage.getItem('token')
  }

  decodedToken(){
    const jwthelper = new JwtHelperService();
    const token = this.getToken()!;
    console.log(token);
    console.log(jwthelper.decodeToken(token) + "################################");
    return  jwthelper.decodeToken(token);
  }

  getuserNameFromToken(){
    if(this.userPayload)
            console.log(this.userPayload.role + "Salom Shox");
            return this.userPayload.role;
  }

  getroleFromToken(){
    if(this.userPayload)
    console.log(this.userPayload.role + "Salom Shox");
            return this.userPayload.role;
  }
}
