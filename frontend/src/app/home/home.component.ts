import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  moviesInTheaters: any;
  moviesFutureReleases: any;

  ngOnInit(): void {
    this.moviesInTheaters = [
      {
        title: 'Spider-Man',
        releaseDate: new Date(),
        price: 1400.99,
        poster: ''
      },
      {
        title: 'Moana',
        releaseDate: new Date('2016-11-14'),
        price: 300.99,
        poster: ''
      }
    ];

    this.moviesFutureReleases = [];
  }

}
