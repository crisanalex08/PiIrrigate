import { Component } from '@angular/core';
import { DataService } from '../../services/data.service';
import { DataModel } from '../../common/models/data.model';
import { TableModule } from 'primeng/table';

@Component({
  selector: 'app-table-data-view',
  imports: [TableModule],
  templateUrl: './table-data-view.component.html',
  styleUrl: './table-data-view.component.css'
})
export class TableDataViewComponent {
  data: DataModel[] ; 
  constructor(private dataService: DataService) {
    this.data = this.dataService.getMockData();
   }

  ngOnInit() {
  }
}
