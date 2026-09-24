import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  email = 'admin@smartmetering.local';
  password = '';
  errorMessage = '';
  loading = false;

  constructor(private auth: AuthService, private router: Router) {}

  submit(): void {
    this.errorMessage = '';
    this.loading = true;

    this.auth.login(this.email, this.password).subscribe({
      next: () => this.router.navigate(['/dashboard']),
      error: () => {
        this.errorMessage = 'Email ou mot de passe incorrect.';
        this.loading = false;
      }
    });
  }
}
