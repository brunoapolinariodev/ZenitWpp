import { Routes } from '@angular/router';

export const automationRoutes: Routes = [
  {
    path: '',
    loadComponent: () => import('./automation-page/automation.component').then(m => m.AutomationPageComponent),
  },
];
