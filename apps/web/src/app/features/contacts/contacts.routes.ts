import { Routes } from '@angular/router';

export const contactsRoutes: Routes = [
  {
    path: '',
    loadComponent: () => import('./contacts-page/contacts.component').then(m => m.ContactsPageComponent),
  },
];
