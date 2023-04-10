import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private baseUrlProduct : string = "https://localhost:7048/api/Product/"

  constructor(private http:HttpClient) { }

  getProduct(){
    return this.http.get<any>(this.baseUrlProduct);
  }

  postProduct(data:any){
    return this.http.post<any>(`${this.baseUrlProduct}`,data);
  }

  putProduct(id:number, data:any){
    return this.http.put<any>(`${this.baseUrlProduct}` +  id, data);
  }

  deleteProduct(id:number){
    return this.http.delete<any>(`${this.baseUrlProduct}`+ id);
   }

}
