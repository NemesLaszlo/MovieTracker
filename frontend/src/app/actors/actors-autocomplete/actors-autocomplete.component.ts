import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatTable } from '@angular/material/table';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { CdkDragDrop, moveItemInArray  } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-actors-autocomplete',
  templateUrl: './actors-autocomplete.component.html',
  styleUrls: ['./actors-autocomplete.component.css']
})
export class ActorsAutocompleteComponent implements OnInit {
  @ViewChild(MatTable) table: MatTable<any> | undefined;
  control: FormControl = new FormControl();
  columnsToDisplay = ['picture', 'name', 'character', 'actions']; // for the "Select the Actors" table - columns

  actors = [
    {name: 'Tom Holland', picture: 'https://m.media-amazon.com/images/M/MV5BNTAzMzA3NjQwOF5BMl5BanBnXkFtZTgwMDUzODQ5MTI@._V1_UY317_CR23,0,214,317_AL_.jpg'},
    {name: 'Tom Hanks', picture: 'https://m.media-amazon.com/images/M/MV5BMTQ2MjMwNDA3Nl5BMl5BanBnXkFtZTcwMTA2NDY3NQ@@._V1_UY317_CR2,0,214,317_AL_.jpg'},
    {name: 'Samuel L. Jackson', picture: 'https://m.media-amazon.com/images/M/MV5BMTQ1NTQwMTYxNl5BMl5BanBnXkFtZTYwMjA1MzY1._V1_UX214_CR0,0,214,317_AL_.jpg'}
  ]

  selectedActors: any = [];
  originalActors = this.actors;

  constructor() { }

  ngOnInit(): void {
    this.control.valueChanges.subscribe(value => {
      this.actors = this.originalActors;
      this.actors = this.actors.filter(actor => actor.name.toLowerCase().indexOf(value) !== -1);
    })
  }

  optionSelected(event: MatAutocompleteSelectedEvent){
    // console.log(event.option.value);
    if(!this.selectedActors.includes(event.option.value)) {
      this.selectedActors.push(event.option.value);
    }
    this.control.patchValue('');
    if (this.table !== undefined){
      this.table.renderRows();
    }
  }

  remove(actor: any){
    const index = this.selectedActors.findIndex((a: { name: string; }) => a.name === actor.name);
    this.selectedActors.splice(index, 1);
    this.table!.renderRows();
  }

  dropped(event: CdkDragDrop<any[]>){
    const previousIndex = this.selectedActors.findIndex((actor: any) => actor === event.item.data);
    moveItemInArray(this.selectedActors, previousIndex, event.currentIndex);
    this.table!.renderRows();
  }

}
