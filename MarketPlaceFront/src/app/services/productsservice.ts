import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Product } from '../Models/Product';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})

export class Productsservice {

  public constructor(private http: HttpClient) { }

  public getProducts(): Observable<Product[]> {

    return this.http.get<Product[]>(`${environment.apiUrl}/product`);
  }
}
export type { Product };

