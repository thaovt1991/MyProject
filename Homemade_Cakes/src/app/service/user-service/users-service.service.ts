import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, throwError } from 'rxjs';
import { RequestModel } from 'src/app/models/request-model';
import { environment } from 'src/environments/environment';

const API_URL = `${environment.apiUrl}`;
@Injectable({
  providedIn: 'root'
})
export class UsersServiceService {
  constructor(private http : HttpClient) { 

  }

  public getOneAsync(id): Observable<any> {
    return this.http.get<any>(`${API_URL}/User/${id}`);
  }

  public getAllAsync(): Observable<any[]> {
    return this.http.get<any[]>(API_URL + '/User');
  }

  public logInAsync(userID: string , password :string): Observable<any> {
    return this.http.post<any>(API_URL + '/User', userID);
  }

  exec(
    assemblyName: string,
    className: string,
    methodName: string,
    data: Array<any>
  ): Observable<any> {
    const host = API_URL + '/Base/invokers';
    let request = new RequestModel();
    request.assemblyName = assemblyName;
    request.className = className;
    request.methodName = methodName;
    request.data = data;
    debugger
    return this.http.post<any>(host, request).pipe(
      map((response: any) => response),
      catchError(this.handleError)
    );
  }
  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'Unknown error!';
    console.error('An error occurred:', error);
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    return throwError(() => new Error(errorMessage));
  }
}
