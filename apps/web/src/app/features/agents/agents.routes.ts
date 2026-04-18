import { Routes } from '@angular/router';

export const agentsRoutes: Routes = [
  {
    path: '',
    loadComponent: () => import('./agents-page/agents.component').then(m => m.AgentsPageComponent),
  },
];
