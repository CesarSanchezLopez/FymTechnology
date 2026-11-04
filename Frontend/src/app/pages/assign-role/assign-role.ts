
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { MessageModule } from 'primeng/message';
import { forkJoin, finalize, timeout } from 'rxjs';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-assign-role',
  standalone: true,
  imports: [FormsModule, CardModule, ButtonModule, MessageModule],
  templateUrl: './assign-role.html'
})
export class AssignRole implements OnInit {

  private api = environment.apiUrl;

  users: any[] = [];
  roles: any[] = [];

  userId = '';
  roleId = '';

  message = '';
  error = '';

  loading = false;
  assigning = false;

  constructor(private http: HttpClient) {}

  private get headers(): HttpHeaders {
    return new HttpHeaders({
      Authorization: `Bearer ${localStorage.getItem('fym_token')}`
    });
  }

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.loading = true;
    this.error = '';

    forkJoin({
      users: this.http.get<any[]>(`${this.api}/Users`, {
        headers: this.headers
      }),
      roles: this.http.get<any[]>(`${this.api}/Roles`, {
        headers: this.headers
      })
    }).pipe(
      timeout(15000),
      finalize(() => this.loading = false)
    ).subscribe({
      next: result => {
        this.users = result.users;
        this.roles = result.roles;
      },
      error: err => {
        console.error('Error cargando datos:', err);

        this.error = err.name === 'TimeoutError'
          ? 'La carga de usuarios y roles tardó demasiado.'
          : err.status === 0
            ? 'No fue posible conectar con el servidor.'
            : 'No fue posible cargar los usuarios y roles.';
      }
    });
  }

  get selectedUser(): any {
    return this.users.find(user => user.id === this.userId);
  }

assign(): void {
  this.message = '';
  this.error = '';

  if (!this.userId || !this.roleId) {
    this.error = 'Selecciona un usuario y un rol.';
    return;
  }

  if (this.assigning) {
    return;
  }

  const selectedRole = this.roles.find(
    role => role.id === this.roleId
  );

  const roleName = selectedRole?.name;

  this.assigning = true;

  this.http.put(
    `${this.api}/Users/${this.userId}/roles/${this.roleId}`,
    {},
    { headers: this.headers }
  ).pipe(
    timeout(15000),
    finalize(() => this.assigning = false)
  ).subscribe({
    next: () => {
      // Actualizar localmente el rol del usuario.
      const user = this.users.find(
        user => user.id === this.userId
      );

      if (user) {
        user.roles = [...(user.roles || []), roleName];
      }

      this.message = `Rol "${roleName ?? 'seleccionado'}" asignado correctamente.`;

      this.roleId = '';
    },
    error: err => {
      console.error('Error asignando rol:', err);

      if (err.name === 'TimeoutError') {
        this.error = 'La operación tardó demasiado. Verifica si el rol fue asignado antes de intentarlo nuevamente.';
      } else if (err.status === 0) {
        this.error = 'No fue posible conectar con el servidor.';
      } else if (Array.isArray(err.error)) {
        this.error = err.error
          .map((e: any) => e.errorMessage || e.message || JSON.stringify(e))
          .join(' | ');
      } else {
        this.error =
          err.error?.detail ||
          err.error?.message ||
          err.error?.title ||
          (typeof err.error === 'string' ? err.error : null) ||
          `Error asignando el rol. Código HTTP: ${err.status}`;
      }
    }
  });
}

}
