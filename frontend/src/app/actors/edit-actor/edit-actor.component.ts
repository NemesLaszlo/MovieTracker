import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { actorCreationDTO, actorDTO } from '../actors.model';
import { ActorsService } from '../actors.service';

@Component({
  selector: 'app-edit-actor',
  templateUrl: './edit-actor.component.html',
  styleUrls: ['./edit-actor.component.css']
})
export class EditActorComponent implements OnInit {

  model: actorDTO | undefined;

  constructor(private activateRoute: ActivatedRoute, private actorsService: ActorsService, private router: Router) { }

  ngOnInit(): void {
    this.activateRoute.params.subscribe(params => {
      this.actorsService.getById(params['id']).subscribe(actor => this.model = actor);
    });
  }

  saveChanges(actorCreationDTO: actorCreationDTO){
    console.log(actorCreationDTO);
    this.actorsService.edit(this.model!.id, actorCreationDTO).subscribe(() => {
      this.router.navigate(['/actors']);
    });
  }

}
