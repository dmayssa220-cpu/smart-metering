import { Routes } from '@angular/router';
import { ShellComponent } from './core/layout/shell/shell.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { MetersComponent } from './features/meters/meters.component';
import { AlertsComponent } from './features/alerts/alerts.component';
import { LoginComponent } from './features/login/login.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent, title: 'Connexion' },
  {
    path: '',
    component: ShellComponent,
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent, title: 'Tableau de bord' },
      { path: 'meters', component: MetersComponent, title: 'Compteurs' },
      { path: 'alerts', component: AlertsComponent, title: 'Alertes' }
    ]
  }
];
