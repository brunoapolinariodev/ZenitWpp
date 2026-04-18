import { Routes } from '@angular/router';

export const campaignsRoutes: Routes = [
  {
    path: '',
    loadComponent: () => import('./campaigns-page/campaigns.component').then(m => m.CampaignsPageComponent),
  },
];
