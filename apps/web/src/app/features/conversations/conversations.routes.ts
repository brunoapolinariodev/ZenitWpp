import { Routes } from '@angular/router';

export const conversationsRoutes: Routes = [
  {
    path: '',
    loadComponent: () => import('./conversations-shell/conversations-shell.component').then(m => m.ConversationsShellComponent),
  },
];
