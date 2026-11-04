import { Component, ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router, RouterLink, RouterOutlet } from '@angular/router';

import { CardModule } from 'primeng/card';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { ButtonModule } from 'primeng/button';
import { MessageModule } from 'primeng/message';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    FormsModule,
    RouterLink,
    RouterOutlet,
    CardModule,
    InputTextModule,
    PasswordModule,
    ButtonModule,
    MessageModule
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  email = 'superadmin@fymtechnology.com';
  password = '';
  loading = false;
  errorMessage = '';
  token = localStorage.getItem('fym_token') || '';

  private readonly apiUrl = 'https://localhost:7022/api';

  constructor(
    private http: HttpClient,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  login(): void {
    this.loading = true;
    this.errorMessage = '';

    this.http.post<any>(`${this.apiUrl}/Auth/login`, {
      email: this.email,
      password: this.password
    }).subscribe({
      next: response => {
        this.token = response.token;
        localStorage.setItem('fym_token', this.token);
        this.loading = false;
        this.router.navigate(['/dashboard']);
        this.cdr.detectChanges();
      },
      error: error => {
        this.errorMessage = error.error?.detail
          || error.error?.message
          || 'No fue posible iniciar sesión.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  logout(): void {
    this.token = '';
    localStorage.removeItem('fym_token');
    this.password = '';
    this.router.navigate(['/']);
  }
}