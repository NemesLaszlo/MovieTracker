import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { coordinatesMap } from 'src/app/utilities/map/coordinate';
import { movieTheatersCreationDTO, movieTheatersDTO } from '../movie-theaters.model';
import { MovieTheatersService } from '../movie-theaters.service';

@Component({
  selector: 'app-edit-movie-theater',
  templateUrl: './edit-movie-theater.component.html',
  styleUrls: ['./edit-movie-theater.component.css']
})
export class EditMovieTheaterComponent implements OnInit {
  model: movieTheatersDTO | undefined;

  constructor(private activatedRoute: ActivatedRoute, private movieTheaterService: MovieTheatersService,
    private router: Router) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(params => {
      this.movieTheaterService.getById(params['id']).subscribe(movieTheater => this.model = movieTheater);
    });
  }

  saveChanges(movieTheater: movieTheatersCreationDTO){
    this.movieTheaterService.edit(this.model!.id, movieTheater).subscribe(() => 
    this.router.navigate(['/movietheaters']));
  }

}
