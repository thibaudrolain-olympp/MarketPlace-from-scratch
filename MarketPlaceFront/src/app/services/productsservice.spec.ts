import { TestBed } from '@angular/core/testing';

import { Productsservice } from './productsservice';

describe('Productsservice', () => {
  let service: Productsservice;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Productsservice);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
