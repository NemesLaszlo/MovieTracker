import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { actorCreationDTO } from '../actors.model';
import { ActorsService } from '../actors.service';

@Component({
  selector: 'app-create-actor',
  templateUrl: './create-actor.component.html',
  styleUrls: ['./create-actor.component.css']
})
export class CreateActorComponent implements OnInit {

  constructor(private router: Router, private actorsService: ActorsService) { }

  ngOnInit(): void {
  }

  saveChanges(actorCreationDTO: actorCreationDTO) {
    this.actorsService.create(actorCreationDTO).subscribe(() => {
      this.router.navigate(['/actors']);
    });
  }

}
