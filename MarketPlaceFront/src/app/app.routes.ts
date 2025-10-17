import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home';
import { LoginComponent } from './pages/login/login';
import { AuthGuard } from './guards/auth-guard-guard';
import { AuthLayoutComponent } from './pages/auth-layout/auth-layout';
import { RegisterComponent } from './pages/register/register';

export const routes: Routes = [
  { path: '', redirectTo: 'auth', pathMatch: 'full' },
  { 
    path: 'auth',
    component: AuthLayoutComponent,
    children: [
      { path: '', redirectTo: 'login', pathMatch: 'full' },
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent }
    ]
  },
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
];