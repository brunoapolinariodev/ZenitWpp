import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { ThemeService } from '../../../core/services/theme.service';
import { AuthService } from '../../../core/services/auth.service';

interface NavItem {
  route: string;
  label: string;
  icon: string;
}

@Component({
  selector: 'app-sidenav',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  template: `
    <nav class="flex flex-col items-center w-16 h-screen
                bg-white dark:bg-slate-900
                border-r border-slate-100 dark:border-slate-800
                py-4 gap-1 shrink-0">

      <!-- Logo -->
      <div class="flex items-center justify-center w-10 h-10 mb-4
                  bg-primary-600 rounded-xl">
        <span class="text-white font-bold text-sm">Z</span>
      </div>

      <!-- Nav items -->
      @for (item of navItems; track item.route) {
        <a [routerLink]="item.route"
           routerLinkActive="bg-primary-50 dark:bg-primary-950 text-primary-600 dark:text-primary-400"
           [routerLinkActiveOptions]="{ exact: false }"
           class="relative group flex items-center justify-center w-10 h-10 rounded-xl
                  text-slate-500 dark:text-slate-400
                  hover:bg-slate-100 dark:hover:bg-slate-800
                  hover:text-slate-700 dark:hover:text-slate-200
                  transition-colors duration-150"
           [title]="item.label">
          <span class="text-lg" [innerHTML]="item.icon"></span>
          <!-- Tooltip -->
          <span class="absolute left-14 px-2 py-1 text-xs font-medium rounded-md
                       bg-slate-900 dark:bg-slate-700 text-white
                       opacity-0 group-hover:opacity-100 pointer-events-none
                       transition-opacity duration-150 whitespace-nowrap z-50">
            {{ item.label }}
          </span>
        </a>
      }

      <div class="flex-1"></div>

      <!-- Theme toggle -->
      <button (click)="theme.toggle()"
              class="flex items-center justify-center w-10 h-10 rounded-xl
                     text-slate-500 dark:text-slate-400
                     hover:bg-slate-100 dark:hover:bg-slate-800
                     transition-colors duration-150"
              title="Alternar tema">
        @if (theme.theme() === 'dark') {
          <span>☀️</span>
        } @else {
          <span>🌙</span>
        }
      </button>

      <!-- Avatar / Logout -->
      <button (click)="auth.logout()"
              class="flex items-center justify-center w-10 h-10 rounded-xl
                     bg-slate-100 dark:bg-slate-800
                     text-slate-600 dark:text-slate-300
                     hover:bg-red-50 dark:hover:bg-red-950
                     hover:text-red-600 dark:hover:text-red-400
                     transition-colors duration-150 mt-1"
              title="Sair">
        <span>⏻</span>
      </button>
    </nav>
  `,
})
export class SidenavComponent {
  theme = inject(ThemeService);
  auth = inject(AuthService);

  navItems: NavItem[] = [
    { route: '/conversations', label: 'Conversas',  icon: '💬' },
    { route: '/contacts',     label: 'Contatos',    icon: '👥' },
    { route: '/dashboard',    label: 'Dashboard',   icon: '📊' },
    { route: '/campaigns',    label: 'Campanhas',   icon: '📣' },
    { route: '/automation',   label: 'Automação',   icon: '⚡' },
    { route: '/reports',      label: 'Relatórios',  icon: '📈' },
    { route: '/agents',       label: 'Agentes',     icon: '🧑‍💼' },
  ];
}
