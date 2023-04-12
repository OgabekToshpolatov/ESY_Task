import { MatDialog ,MatDialogRef,MAT_DIALOG_DATA} from '@angular/material/dialog';
import { Component,OnInit,ViewChild } from '@angular/core';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import { ApiService } from 'src/app/services/api.service';
import { DialogComponent } from '../dialog/dialog.component';
import { AuthService } from 'src/app/services/auth.service';
import { UserStoreService } from 'src/app/services/user-store.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit{

  public role :string ="";
  displayedColumns: string[] = [];

  constructor(
    private api:ApiService,
    private auth:AuthService,
    private dialog:MatDialog,
    private userStore:UserStoreService,
    private toastr:ToastrService
     )
     {
      this.userStore.getRoleFromStore()
      .subscribe(val => {
        let roleFromToken = this.auth.getroleFromToken();
        this.role = val || roleFromToken
      })
      if(this.role === "admin"){
         this.displayedColumns = ['title', 'quantity', 'price', 'totalPriceWithVat','action'] ;
      }else{
         this.displayedColumns = ['title', 'quantity', 'price', 'totalPriceWithVat']
      }
     }

  dataSource!: MatTableDataSource<any>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit(): void {

    this.userStore.getRoleFromStore()
        .subscribe(val => {
          let roleFromToken = this.auth.getroleFromToken();
          this.role = val || roleFromToken
        })

        this.getAllProducts()
  }

  logout(){
     this.auth.signOut();
  }

  openDialog(){
    this.dialog.open(DialogComponent,{
      width:"30%"
    })
    .afterClosed().subscribe(res => {
      if(res ==="Save"){
        this.getAllProducts();
      }
    });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  getAllProducts(){
    return this.api.getProduct()
      .subscribe({
         next:(res) => {
           this.dataSource = new MatTableDataSource(res)
           this.dataSource.paginator = this.paginator
           this.dataSource.sort = this.sort
         },
         error:() => {
           alert("Something went wrong getAll Products");
         }
      })
  }

  editProduct(row:any){
    this.dialog.open(DialogComponent,{
      width:'30%',
      data:row
    }).afterClosed().subscribe(res => {
      if(res ==="update"){
        this.getAllProducts();
      }
    })
  }

  deleteProduct(id:number){
    this.api.deleteProduct(id)
        .subscribe({
           next:(res) => {
            alert("Product deleted succesfully")
            this.toastr.success("Product deleted succesfully")
             this.getAllProducts()
          },
           error:() => {
            this.toastr.error("Something went wrong while delete product")
        }
      }
    )
  }
}
