import { Routes } from '@angular/router';

import { Dashboard } from './pages/dashboard/dashboard';
import { Users } from './pages/users/users';
import { AssignRole } from './pages/assign-role/assign-role';

export const routes: Routes = [
  { path: 'dashboard', component: Dashboard },
  { path: 'users', component: Users },
  { path: 'assign-role', component: AssignRole },
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: '**', redirectTo: 'dashboard' }
];