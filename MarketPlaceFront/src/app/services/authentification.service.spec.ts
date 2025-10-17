import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AuthService } from './authentification';
import { environment } from '../../environments/environment';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [AuthService]
    });

    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);

    // clear localStorage before each test
    localStorage.clear();
  });

  afterEach(() => {
    httpMock.verify();
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('getBearerToken should POST to bearerUrl and store bearer in localStorage', (done) => {
    const mock = { token: 'bearer-token' };

    service.getBearerToken().subscribe(res => {
      expect(res.token).toBe('bearer-token');
      expect(localStorage.getItem('bearer')).toBe('bearer-token');
      done();
    });

    const req = httpMock.expectOne(environment.bearerUrl);
    expect(req.request.method).toBe('POST');
    req.flush(mock);
  });

  it('logout should remove token from localStorage and isLoggedIn should reflect that', () => {
    localStorage.setItem('token', 'abc');
    expect(service.isLoggedIn()).toBeTrue();
    service.logout();
    expect(service.getToken()).toBeNull();
    expect(service.isLoggedIn()).toBeFalse();
  });
});
