import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { switchMap } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) {}

getBearerToken(): Observable<{ token: string }> {
    return this.http.post<{ token: string }>(
      environment.bearerUrl,
      { Key: environment.apiKey },
      { headers: { 'Content-Type': 'application/json' } }
  ).pipe(
    tap(res => localStorage.setItem('bearer', res.token)),
  );
  }

loginWithBearer(username: string, password: string):  Observable<{ token: string }> {
  return this.getBearerToken().pipe(
    switchMap(() => {
      const bearer = localStorage.getItem('bearer');
      return this.http.post<{ token: string }>(
        `${environment.apiUrl}/GestionComptes/login`,
        { username, password },
        { headers: { 'Authorization': `Bearer ${bearer}` } }
      ).pipe(
        tap(res => localStorage.setItem('token', res.token))
      );
    })
  );
}

register(username: string, email: string, password: string, phoneNumber: string) {
  return this.http.post(`${environment.apiUrl}/GestionComptes/Register`, {
    username,
    email,
    password,
    phoneNumber
  },
{ observe: 'response', responseType: 'text' });
  
}

  logout(): void {
    localStorage.removeItem('token');
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }
}
