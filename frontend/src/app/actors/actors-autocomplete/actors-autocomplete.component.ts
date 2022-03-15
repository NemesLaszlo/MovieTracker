import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatTable } from '@angular/material/table';
import { MatAutocompleteSelectedEvent } from '@angular/material/autocomplete';
import { CdkDragDrop, moveItemInArray  } from '@angular/cdk/drag-drop';
import { ActorsService } from '../actors.service';
import { actorsMovieDTO } from '../actors.model';

@Component({
  selector: 'app-actors-autocomplete',
  templateUrl: './actors-autocomplete.component.html',
  styleUrls: ['./actors-autocomplete.component.css']
})
export class ActorsAutocompleteComponent implements OnInit {
  @ViewChild(MatTable) table: MatTable<any> | undefined;
  @Input() selectedActors: actorsMovieDTO[] = [];
  actorsToDisplay: actorsMovieDTO[] = [];
  control: FormControl = new FormControl();
  columnsToDisplay = ['picture', 'name', 'character', 'actions']; // for the "Select the Actors" table - columns


  constructor(private actorsService: ActorsService) { }

  ngOnInit(): void {
    this.control.valueChanges.subscribe(value => {
      this.actorsService.searchByName(value).subscribe(actors => {
        this.actorsToDisplay = actors;
      });
    })
  }

  optionSelected(event: MatAutocompleteSelectedEvent){
    // console.log(event.option.value);
    this.control.patchValue('');
    
    // duplicate select avoid
    if (this.selectedActors.findIndex(x => x.id == event.option.value.id) !== -1){
      return;
    }

    this.selectedActors.push(event.option.value);

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
