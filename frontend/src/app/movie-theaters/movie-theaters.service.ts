import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { movieTheatersCreationDTO, movieTheatersDTO } from './movie-theaters.model';

@Injectable({
  providedIn: 'root'
})
export class MovieTheatersService {
  private apiURL = environment.apiURL + '/movieTheaters';

  constructor(private http: HttpClient) { }

  get(): Observable<movieTheatersDTO[]>{
    return this.http.get<movieTheatersDTO[]>(this.apiURL);
  }

  getById(id: number): Observable<movieTheatersDTO>{
    return this.http.get<movieTheatersDTO>(`${this.apiURL}/${id}`);
  }

  create(movieTheaterDTO: movieTheatersCreationDTO){
    return this.http.post(this.apiURL, movieTheaterDTO);
  }

  edit(id: number, movieTheaterDTO: movieTheatersCreationDTO){
    return this.http.put(`${this.apiURL}/${id}`, movieTheaterDTO);
  }

  delete(id: number){
    return this.http.delete(`${this.apiURL}/${id}`);
  }
}
