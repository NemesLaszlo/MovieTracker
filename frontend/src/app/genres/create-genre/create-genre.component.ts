import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { parseWebAPIErrors } from 'src/app/utilities/utils';
import { genreCreationDTO } from '../genres.model';
import { GenresService } from '../genres.service';

@Component({
  selector: 'app-create-genre',
  templateUrl: './create-genre.component.html',
  styleUrls: ['./create-genre.component.css']
})
export class CreateGenreComponent implements OnInit {
  errors: string[] = [];

  constructor(private router: Router, private genresService: GenresService) { }

  ngOnInit(): void {
  }

  saveChanges(genreCreationDTO: genreCreationDTO) {
    this.genresService.create(genreCreationDTO).subscribe(() => {
      this.router.navigate(['/genres']);
    }, error => this.errors = parseWebAPIErrors(error));
  }

}
