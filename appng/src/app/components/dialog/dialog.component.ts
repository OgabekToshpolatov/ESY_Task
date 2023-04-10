import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ApiService } from './../../services/api.service';
import { Component, Inject, OnInit} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.css']
})
export class DialogComponent implements OnInit{

  productForm!:FormGroup
  actionBtn:string="Save"
  actionName:string="Add Product"
  constructor(
    private api:ApiService,
    private formBuilder:FormBuilder,
    private matdialog:MatDialogRef<DialogComponent>,
    @Inject(MAT_DIALOG_DATA) public editData:any
  ) { }

  ngOnInit(): void {
    this.productForm = this.formBuilder.group({
        title:["",Validators.required],
        quantity:["",Validators.required],
        price:["",Validators.required],
    })
    if(this.editData){
      this.actionBtn = "Update"
      this.actionName = "Update Product"
      this.productForm.controls['title'].setValue(this.editData.title);
      this.productForm.controls['quantity'].setValue(this.editData.quantity);
      this.productForm.controls['price'].setValue(this.editData.price);
    }
  }

  addProduct(){
    if(!this.editData)
    {
      if(this.productForm.valid)
      {
        this.api.postProduct(this.productForm.value)
        .subscribe({
         next:() => {
           alert(" Product was added succesfully ")
            this.productForm.reset()
            this.matdialog.close("Save");
         },
         error:(err) => {
           alert(err?.error.message)
           this.matdialog.close();
         }
        })
      }
    }
    else{
      this.updateProduct()
    }

  }

  updateProduct(){
     this.api.putProduct(this.editData.id, this.productForm.value)
         .subscribe({
             next:(res) => {
                 alert("Product update succesfully")
                 this.productForm.reset()
                 this.matdialog.close("update")
             },
             error:() => {
              alert("Something went wrong while updating")
             }
         })
  }


}
