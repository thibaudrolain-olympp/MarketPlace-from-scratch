import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/authentification';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './register.html',
  styleUrls: ['./register.css']
})
export class RegisterComponent {
  public username: string = '';
  public email: string = '';
  public phoneNumber: string = '';
  public password: string = '';
  public passwordConfirm: string = '';
  public successMessage: string = ''; 
  public errorMessage: string = '';    

  constructor(private auth: AuthService, private router: Router) {}

  onRegister() {
    if (this.password !== this.passwordConfirm) {
      this.errorMessage = "Les mots de passe ne correspondent pas.";
      return;
    }

    this.auth.register(this.username, this.email, this.password, this.phoneNumber).subscribe({
      next: (res) => {
        if (res.status === 204 || res.status === 200) {
        alert('Inscription réussie ! Veuillez vérifier votre mail pour confirmer.');
        this.router.navigate(['/auth/login']);
      }
      },
      error: (err) => {
        this.errorMessage = err.error || "Erreur lors de l'inscription.";
      }
    });
  }
}
