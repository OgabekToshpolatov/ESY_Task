import { MatDialog ,MAT_DIALOG_DATA} from '@angular/material/dialog';
import { Component,OnInit,ViewChild } from '@angular/core';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';
import { ApiService } from './services/api.service'
import { MatInputModule } from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import { DialogComponent } from './components/dialog/dialog.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers:[ApiService]
})
export class AppComponent implements OnInit {
  title = 'appng';

  constructor(private dialog:MatDialog,private api:ApiService, private router:Router){}

  displayedColumns: string[] = ['title', 'quantity', 'price', 'totalPriceWithVat'];
  dataSource!: MatTableDataSource<any>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit():void{
    // this.getAllProducts();
  }

  openDialog(){
    this.dialog.open(DialogComponent,{
      width:"30%"
    });
  }

  openSignUpDialog(){
    this.router.navigate(['signup'])
    console.log("salom_________________________________________________________________________________________");
  }
  openSignInDialog(){
    this.router.navigate(['signin'])
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

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
}
