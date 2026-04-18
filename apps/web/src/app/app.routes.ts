import { Routes } from '@angular/router';
import { authGuard, publicGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'auth',
    canActivate: [publicGuard],
    loadChildren: () => import('./features/auth/auth.routes').then(m => m.authRoutes),
  },
  {
    path: '',
    canActivate: [authGuard],
    loadComponent: () => import('./shared/layout/shell/shell.component').then(m => m.ShellComponent),
    children: [
      { path: '', redirectTo: 'conversations', pathMatch: 'full' },
      {
        path: 'conversations',
        loadChildren: () => import('./features/conversations/conversations.routes').then(m => m.conversationsRoutes),
      },
      {
        path: 'contacts',
        loadChildren: () => import('./features/contacts/contacts.routes').then(m => m.contactsRoutes),
      },
      {
        path: 'agents',
        loadChildren: () => import('./features/agents/agents.routes').then(m => m.agentsRoutes),
      },
      {
        path: 'automation',
        loadChildren: () => import('./features/automation/automation.routes').then(m => m.automationRoutes),
      },
      {
        path: 'campaigns',
        loadChildren: () => import('./features/campaigns/campaigns.routes').then(m => m.campaignsRoutes),
      },
      {
        path: 'reports',
        loadChildren: () => import('./features/reports/reports.routes').then(m => m.reportsRoutes),
      },
      {
        path: 'dashboard',
        loadChildren: () => import('./features/dashboard/dashboard.routes').then(m => m.dashboardRoutes),
      },
    ],
  },
  { path: '**', redirectTo: 'conversations' },
];
