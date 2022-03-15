import { Component, OnInit } from '@angular/core';
import { movieTheatersDTO } from '../movie-theaters.model';
import { MovieTheatersService } from '../movie-theaters.service';

@Component({
  selector: 'app-index-movie-theater',
  templateUrl: './index-movie-theater.component.html',
  styleUrls: ['./index-movie-theater.component.css']
})
export class IndexMovieTheaterComponent implements OnInit {
  movieTheaters: movieTheatersDTO[] = [];
  displayColumns = ['name', 'actions'];

  constructor(private movieTheatersService: MovieTheatersService) { }

  ngOnInit(): void {
    this.loadData()
  }

  loadData(){
    this.movieTheatersService.get().subscribe(movieTheaters => this.movieTheaters = movieTheaters);
  }

  delete(id: number){
    this.movieTheatersService.delete(id).subscribe(() => this.loadData());
  }

}
