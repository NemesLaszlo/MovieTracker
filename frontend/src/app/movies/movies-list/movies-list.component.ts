import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { movieDTO } from '../movies.model';
import { MoviesService } from '../movies.service';

@Component({
  selector: 'app-movies-list',
  templateUrl: './movies-list.component.html',
  styleUrls: ['./movies-list.component.css']
})
export class MoviesListComponent implements OnInit {
  @Input() movies: movieDTO[] | undefined | null;
  @Output() onDelete = new EventEmitter<void>();

  constructor(private moviesService: MoviesService) { }

  ngOnInit(): void {
  }

  remove(id: number){
    this.moviesService.delete(id).subscribe(() => {
      this.onDelete.emit();
    });
  }

}
