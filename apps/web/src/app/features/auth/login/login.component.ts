import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { ThemeService } from '../../../core/services/theme.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  template: `
    <div class="flex min-h-screen items-center justify-center
                bg-slate-50 dark:bg-slate-900 px-4">

      <!-- Theme toggle (topo direito) -->
      <button (click)="theme.toggle()"
              class="absolute top-4 right-4 btn-ghost text-xl">
        {{ theme.theme() === 'dark' ? '☀️' : '🌙' }}
      </button>

      <div class="w-full max-w-sm">

        <!-- Logo -->
        <div class="flex items-center gap-3 mb-10">
          <div class="flex items-center justify-center w-10 h-10
                      bg-primary-600 rounded-xl">
            <span class="text-white font-bold">Z</span>
          </div>
          <span class="text-xl font-semibold text-slate-900 dark:text-slate-50">
            ZenitWpp
          </span>
        </div>

        <!-- Título -->
        <h1 class="text-2xl font-semibold text-slate-900 dark:text-slate-50 mb-1">
          Bem-vindo de volta
        </h1>
        <p class="text-sm text-slate-500 dark:text-slate-400 mb-8">
          Faça login para acessar o painel
        </p>

        <!-- Formulário -->
        <form [formGroup]="form" (ngSubmit)="submit()" class="space-y-4">

          <div>
            <label class="block text-sm font-medium text-slate-700 dark:text-slate-300 mb-1.5">
              E-mail
            </label>
            <input formControlName="email"
                   type="email"
                   autocomplete="email"
                   placeholder="seu@email.com"
                   class="input" />
          </div>

          <div>
            <label class="block text-sm font-medium text-slate-700 dark:text-slate-300 mb-1.5">
              Senha
            </label>
            <input formControlName="password"
                   type="password"
                   autocomplete="current-password"
                   placeholder="••••••••"
                   class="input" />
          </div>

          @if (requiresTwoFactor()) {
            <div>
              <label class="block text-sm font-medium text-slate-700 dark:text-slate-300 mb-1.5">
                Código 2FA
              </label>
              <input formControlName="totpCode"
                     type="text"
                     inputmode="numeric"
                     maxlength="6"
                     placeholder="000000"
                     class="input tracking-widest text-center text-lg" />
              <p class="mt-1.5 text-xs text-slate-500 dark:text-slate-400">
                Abra o app autenticador e insira o código de 6 dígitos
              </p>
            </div>
          }

          @if (error()) {
            <p class="text-sm text-red-600 dark:text-red-400">{{ error() }}</p>
          }

          <button type="submit"
                  [disabled]="loading()"
                  class="btn-primary w-full justify-center py-2.5">
            @if (loading()) {
              <span class="animate-spin">⏳</span>
              Entrando...
            } @else {
              Entrar
            }
          </button>

        </form>
      </div>
    </div>
  `,
})
export class LoginComponent {
  private auth = inject(AuthService);
  private router = inject(Router);
  private fb = inject(FormBuilder);

  theme = inject(ThemeService);

  loading = signal(false);
  error = signal('');
  requiresTwoFactor = signal(false);

  form = this.fb.group({
    email:    ['', [Validators.required, Validators.email]],
    password: ['', Validators.required],
    totpCode: [''],
  });

  submit(): void {
    if (this.form.invalid) return;

    this.loading.set(true);
    this.error.set('');

    const { email, password, totpCode } = this.form.value;

    this.auth.login({ email: email!, password: password!, totpCode: totpCode || undefined })
      .subscribe({
        next: (res) => {
          if (res.requiresTwoFactor) {
            this.requiresTwoFactor.set(true);
            this.loading.set(false);
          } else {
            this.router.navigate(['/conversations']);
          }
        },
        error: (err) => {
          this.loading.set(false);
          this.error.set(err?.error?.message ?? 'Credenciais inválidas');
        },
      });
  }
}
