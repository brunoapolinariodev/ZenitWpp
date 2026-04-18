import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SidenavComponent } from '../sidenav/sidenav.component';

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [RouterOutlet, SidenavComponent],
  template: `
    <div class="flex h-screen overflow-hidden bg-slate-50 dark:bg-slate-900">
      <app-sidenav />
      <div class="flex flex-1 overflow-hidden">
        <router-outlet />
      </div>
    </div>
  `,
})
export class ShellComponent {}
