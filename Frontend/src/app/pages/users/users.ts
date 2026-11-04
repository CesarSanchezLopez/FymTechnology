import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CardModule } from 'primeng/card';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { ButtonModule } from 'primeng/button';
import { MessageModule } from 'primeng/message';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [
    FormsModule,
    CardModule,
    InputTextModule,
    PasswordModule,
    ButtonModule,
    MessageModule
  ],
  templateUrl: './users.html'
})
export class Users implements OnInit {

  private api = environment.apiUrl;

  users: any[] = [];
  roles: any[] = [];

  error = '';
  success = '';

  newUser = {
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    roleId: ''
  };

  constructor(private http: HttpClient) {}

  private get headers(): HttpHeaders {
    return new HttpHeaders({
      Authorization: `Bearer ${localStorage.getItem('fym_token')}`
    });
  }

  ngOnInit(): void {
    this.loadUsers();
    this.loadRoles();
  }

  loadUsers(): void {
    this.http.get<any[]>(`${this.api}/Users`, {
      headers: this.headers
    }).subscribe({
      next: data => this.users = data,
      error: err => {
        this.error = err.error?.detail || 'Error consultando usuarios.';
      }
    });
  }

  loadRoles(): void {
    this.http.get<any[]>(`${this.api}/Roles`, {
      headers: this.headers
    }).subscribe({
      next: data => this.roles = data,
      error: err => {
        console.error('Error consultando roles:', err);
        this.error = 'Error consultando roles.';
      }
    });
  }

  createUser(): void {
    this.error = '';
    this.success = '';

    this.http.post(`${this.api}/Users`, this.newUser, {
      headers: this.headers
    }).subscribe({
      next: () => {
        this.success = 'Usuario creado correctamente.';

        this.newUser = {
          firstName: '',
          lastName: '',
          email: '',
          password: '',
          roleId: ''
        };

        this.loadUsers();
      },
      error: err => {
        console.error(
          'Respuesta del backend:',
          JSON.stringify(err.error, null, 2)
        );

        if (Array.isArray(err.error)) {
          this.error = err.error
            .map((e: any) => e.errorMessage || e.message || JSON.stringify(e))
            .join(' | ');
        } else {
          this.error = err.error?.detail
            || err.error?.message
            || JSON.stringify(err.error)
            || 'Error creando usuario.';
        }
      }
    });
  }
}