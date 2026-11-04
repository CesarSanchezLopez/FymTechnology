
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { MessageModule } from 'primeng/message';

@Component({
  selector: 'app-assign-role',
  standalone: true,
  imports: [FormsModule, CardModule, ButtonModule, MessageModule],
  templateUrl: './assign-role.html'
})
export class AssignRole implements OnInit {

  private api = 'https://localhost:7022/api';

  users: any[] = [];
  roles: any[] = [];

  userId = '';
  roleId = '';

  message = '';
  error = '';
  loading = false;

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
        console.error('Error consultando usuarios:', err);
        this.error = 'Error consultando usuarios.';
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

  get selectedUser(): any {
    return this.users.find(user => user.id === this.userId);
  }

  assign(): void {
    this.error = '';
    this.message = '';
    this.loading = true;

    this.http.put(
      `${this.api}/Users/${this.userId}/roles/${this.roleId}`,
      {},
      { headers: this.headers }
    ).subscribe({
      next: () => {
        const roleName = this.roles.find(
          role => role.id === this.roleId
        )?.name;

        this.message = `Rol "${roleName}" asignado correctamente.`;
        this.loading = false;

        this.roleId = '';

        // Recargar usuarios para visualizar los roles actualizados.
        this.loadUsers();
      },

      error: err => {
        console.error('Error asignando rol:', err);

        console.error(
          'Detalle del backend:',
          JSON.stringify(err.error, null, 2)
        );

        if (Array.isArray(err.error)) {
          this.error = err.error
            .map((e: any) =>
              e.errorMessage || e.message || JSON.stringify(e)
            )
            .join(' | ');
        } else {
          this.error = err.error?.detail
            || err.error?.message
            || JSON.stringify(err.error)
            || 'Error asignando el rol.';
        }

        this.loading = false;
      }
    });
  }
}

