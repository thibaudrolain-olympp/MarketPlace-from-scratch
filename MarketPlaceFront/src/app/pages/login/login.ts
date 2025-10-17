import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgIf } from '@angular/common';
import { AuthService } from '../../services/authentification';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, NgIf],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent {
  username = '';
  password = '';

  constructor(public auth: AuthService, private router: Router) {}

  onLogin() {
    this.auth.loginWithBearer(this.username, this.password).subscribe({
      next: () => {
        console.log('Login réussi, token utilisateur:', this.auth.getToken());
        this.router.navigate(['/home']); // navigue après que le token utilisateur soit stocké
      },
      error: err => console.error('Erreur login', err)
    });
  }
  onLogout() {
    this.auth.logout();
    this.router.navigate(['/auth/login']);
  }
}
