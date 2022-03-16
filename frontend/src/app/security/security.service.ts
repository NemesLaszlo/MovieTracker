import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { authenticationResponse, userDTO, userCredentials } from './security.models';

@Injectable({
  providedIn: 'root'
})
export class SecurityService {
  private apiURL = environment.apiURL + "/accounts";
  private readonly tokenKey: string = 'token';
  private readonly expirationTokenKey: string = 'token-expiration'
  private readonly roleField = "role";

  constructor(private http: HttpClient) { }

  getUsers(page: number, recordsPerPage: number): Observable<any>{
    let params = new HttpParams();
    params = params.append('page', page.toString());
    params = params.append('recordsPerPage', recordsPerPage.toString());
    return this.http.get<userDTO[]>(`${this.apiURL}/listusers`,{observe: 'response', params});
  }

  makeAdmin(userId: string){
    const headers = new HttpHeaders('Content-Type: application/json');
    return this.http.post(`${this.apiURL}/makeadmin`, JSON.stringify(userId), {headers});
  }

  removeAdmin(userId: string){
    const headers = new HttpHeaders('Content-Type: application/json');
    return this.http.post(`${this.apiURL}/removeadmin`, JSON.stringify(userId), {headers});
  }

  isAuthenticated(): boolean{
    const token = localStorage.getItem(this.tokenKey);

    if (!token){
      return false;
    }

    const expiration: string = localStorage.getItem(this.expirationTokenKey)!;
    const expirationDate = new Date(expiration);

    if (expirationDate <= new Date()){
      this.logout();
      return false;
    }

    return true;
  }

  getFieldFromJWT(field: string): string {
    const token = localStorage.getItem(this.tokenKey);
    if (!token){return '';}
    const dataToken = JSON.parse(atob(token.split('.')[1]));
    return dataToken[field];
  }

  logout(){
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.expirationTokenKey);
  }

  getRole(): string {
    return this.getFieldFromJWT(this.roleField);
  }

  register(userCredentials: userCredentials): Observable<authenticationResponse>{
    return this.http.post<authenticationResponse>(this.apiURL + "/create", userCredentials);
  }

  login(userCredentials: userCredentials): Observable<authenticationResponse>{
    return this.http.post<authenticationResponse>(this.apiURL + "/login", userCredentials);
  }

  saveToken(authenticationResponse: authenticationResponse){
    localStorage.setItem(this.tokenKey, authenticationResponse.token);
    localStorage.setItem(this.expirationTokenKey, authenticationResponse.expiration.toString());
  }

  getToken(){
    return localStorage.getItem(this.tokenKey);
  }
}
