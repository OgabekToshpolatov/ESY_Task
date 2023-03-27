import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ApiService } from './../../services/api.service';
import { Component, OnInit} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.css']
})
export class DialogComponent implements OnInit{

  productForm!:FormGroup
  constructor(
    private api:ApiService,
    private formBuilder:FormBuilder,
    private matdialog:MatDialogRef<DialogComponent>,

  ) { }

  ngOnInit(): void {
    this.productForm = this.formBuilder.group({
        title:["",Validators.required],
        quantity:["",Validators.required],
        price:["",Validators.required],
    })
  }

  addProduct(){
    this.api.postProduct(this.productForm.value)
         .subscribe({
          next:() => {
            alert(" Product was added succesfully ")
             this.productForm.reset()
             this.matdialog.close("Save");
          },
          error:() => {
            alert("Something went wrong while adding")
            this.matdialog.close();
          }
         })
  }
}
