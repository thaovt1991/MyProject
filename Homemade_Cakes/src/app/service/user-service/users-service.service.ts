import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

const API_URL = `${environment.apiUrl}`;
@Injectable({
  providedIn: 'root'
})
export class UsersServiceService {
  constructor(private http : HttpClient) { 

  }

  public getOneAsync(id: number): Observable<any> {
    return this.http.get<any>(`${API_URL}/User/${id}`);
  }

  public getAllAsync(): Observable<any[]> {
    return this.http.get<any[]>(API_URL + '/User');
  }

  public logInAsync(userID: string , password :string): Observable<any> {
    return this.http.post<any>(API_URL + '/User', userID);
  }
}
