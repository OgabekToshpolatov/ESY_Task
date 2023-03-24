import { Component,ViewChild } from '@angular/core';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';
import { ApiService } from './services/api.service'
import { MatInputModule } from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers:[ApiService]
})
export class AppComponent {
  title = 'appng';

  constructor(private api:ApiService){}

  displayedColumns: string[] = ['title', 'quantity', 'price', 'totalPriceWithVat'];
  dataSource!: MatTableDataSource<any>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  getAllProducts(){
    return this.api.getProduct()
      .subscribe({
         next:(res) => {
           this.dataSource = new MatTableDataSource(res)
           this.dataSource.paginator = this.paginator
           this.dataSource.sort = this.sort
         },
         error:() => {
           alert("Something went wrong");
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