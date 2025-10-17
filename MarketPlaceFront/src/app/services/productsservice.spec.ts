import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { Productsservice } from './productsservice';
import { environment } from '../../environments/environment';
import { Product } from '../Models/Product';

describe('Productsservice', () => {
  let service: Productsservice;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [Productsservice]
    });
    service = TestBed.inject(Productsservice);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('getProducts should call GET and return products', (done) => {
    const mockProducts: Product[] = [
      { id: 1, name: 'P1', description: 'd1', price: 10, quantity: 1, category: { id: 1, name: 'c1' }, images: [], status: 'active', createdAt: new Date(), updatedAt: new Date() },
      { id: 2, name: 'P2', description: 'd2', price: 20, quantity: 2, category: { id: 2, name: 'c2' }, images: [], status: 'active', createdAt: new Date(), updatedAt: new Date() }
    ];

    service.getProducts().subscribe(products => {
      expect(products.length).toBe(2);
      expect(products).toEqual(mockProducts);
      done();
    });

    const req = httpMock.expectOne(`${environment.apiUrl}/product`);
    expect(req.request.method).toBe('GET');
    req.flush(mockProducts);
  });
});
