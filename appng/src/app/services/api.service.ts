import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private baseUrlProduct : string = "https://localhost:7048/api/Product/"

  private baseUrlAccount : string = "https://localhost:7048/api/Account/"

  constructor(private http:HttpClient) { }

  signup(signUp:any){
    return this.http.post<any>(`${this.baseUrlAccount}signup`,signUp)
  }

  signin(signIn:any){
    return this.http.post<any>(`${this.baseUrlAccount}signin`,signIn)
  }

  getProduct(){
    return this.http.get<any>(this.baseUrlProduct);
  }

  postProduct(data:any){
    return this.http.post<any>(`${this.baseUrlProduct}`,data);
  }

}
