import { Component, inject, signal, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { AgentService, AgentResponse } from '../../../core/services/agent.service';

@Component({
  selector: 'app-agents',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule],
  template: `
    <div class="flex-1 overflow-y-auto bg-slate-50 dark:bg-slate-900/40">
      <div class="max-w-4xl mx-auto px-8 py-8">

        <!-- Header -->
        <div class="flex items-center justify-between mb-8">
          <div>
            <h1 class="text-xl font-semibold text-slate-900 dark:text-slate-50">Agentes</h1>
            <p class="text-sm text-slate-500 dark:text-slate-400 mt-1">Gerencie os agentes de atendimento</p>
          </div>
          <button (click)="showForm.set(true)"
                  class="btn-primary">
            + Novo agente
          </button>
        </div>

        <!-- Formulário de criação -->
        @if (showForm()) {
          <div class="card p-6 mb-6">
            <h2 class="text-sm font-semibold text-slate-900 dark:text-slate-50 mb-4">Novo agente</h2>
            <form [formGroup]="form" (ngSubmit)="create()" class="grid grid-cols-2 gap-4">
              <div>
                <label class="block text-xs font-medium text-slate-600 dark:text-slate-400 mb-1.5">Nome</label>
                <input formControlName="name" class="input" placeholder="Nome completo" />
              </div>
              <div>
                <label class="block text-xs font-medium text-slate-600 dark:text-slate-400 mb-1.5">E-mail</label>
                <input formControlName="email" type="email" class="input" placeholder="agente@empresa.com" />
              </div>
              <div>
                <label class="block text-xs font-medium text-slate-600 dark:text-slate-400 mb-1.5">Senha</label>
                <input formControlName="password" type="password" class="input" placeholder="••••••••" />
              </div>
              <div>
                <label class="block text-xs font-medium text-slate-600 dark:text-slate-400 mb-1.5">Perfil</label>
                <select formControlName="role" class="input">
                  <option value="Operator">Operador</option>
                  <option value="Supervisor">Supervisor</option>
                  <option value="Admin">Admin</option>
                </select>
              </div>
              <div class="col-span-2 flex gap-2 justify-end pt-2">
                <button type="button" (click)="cancelForm()" class="btn-ghost">Cancelar</button>
                <button type="submit" [disabled]="form.invalid || saving()" class="btn-primary">
                  {{ saving() ? 'Salvando...' : 'Criar agente' }}
                </button>
              </div>
            </form>
          </div>
        }

        <!-- Tabela -->
        <div class="card overflow-hidden">
          @if (loading()) {
            <div class="divide-y divide-slate-100 dark:divide-slate-800">
              @for (i of [1,2,3]; track i) {
                <div class="flex items-center gap-4 px-5 py-4 animate-pulse">
                  <div class="w-9 h-9 rounded-full bg-slate-200 dark:bg-slate-700"></div>
                  <div class="flex-1 space-y-2">
                    <div class="h-3 bg-slate-200 dark:bg-slate-700 rounded w-1/4"></div>
                    <div class="h-2.5 bg-slate-100 dark:bg-slate-800 rounded w-1/3"></div>
                  </div>
                </div>
              }
            </div>
          } @else if (agents().length === 0) {
            <div class="flex flex-col items-center justify-center py-16 text-slate-400 dark:text-slate-500">
              <p class="text-sm">Nenhum agente cadastrado</p>
            </div>
          } @else {
            <table class="w-full text-sm">
              <thead>
                <tr class="border-b border-slate-100 dark:border-slate-800">
                  <th class="text-left px-5 py-3 text-xs font-semibold text-slate-500 dark:text-slate-400">Agente</th>
                  <th class="text-left px-5 py-3 text-xs font-semibold text-slate-500 dark:text-slate-400">Perfil</th>
                  <th class="text-left px-5 py-3 text-xs font-semibold text-slate-500 dark:text-slate-400">Status</th>
                  <th class="text-left px-5 py-3 text-xs font-semibold text-slate-500 dark:text-slate-400">2FA</th>
                  <th class="text-left px-5 py-3 text-xs font-semibold text-slate-500 dark:text-slate-400">Desde</th>
                </tr>
              </thead>
              <tbody class="divide-y divide-slate-100 dark:divide-slate-800">
                @for (agent of agents(); track agent.id) {
                  <tr class="hover:bg-slate-50 dark:hover:bg-slate-800/40 transition-colors duration-100">
                    <td class="px-5 py-3.5">
                      <div class="flex items-center gap-3">
                        <div class="w-8 h-8 rounded-full flex items-center justify-center text-xs font-semibold
                                    bg-primary-50 dark:bg-primary-950 text-primary-700 dark:text-primary-300">
                          {{ initials(agent.name) }}
                        </div>
                        <div>
                          <p class="font-medium text-slate-900 dark:text-slate-50">{{ agent.name }}</p>
                          <p class="text-xs text-slate-500 dark:text-slate-400">{{ agent.email }}</p>
                        </div>
                      </div>
                    </td>
                    <td class="px-5 py-3.5">
                      <span class="px-2 py-0.5 rounded-full text-xs font-medium"
                            [class]="roleChip(agent.role)">
                        {{ roleLabel(agent.role) }}
                      </span>
                    </td>
                    <td class="px-5 py-3.5">
                      <div class="flex items-center gap-1.5">
                        <span class="w-1.5 h-1.5 rounded-full"
                              [class]="agent.isOnline ? 'bg-emerald-400' : 'bg-slate-300 dark:bg-slate-600'">
                        </span>
                        <span class="text-xs text-slate-600 dark:text-slate-400">
                          {{ agent.isOnline ? 'Online' : 'Offline' }}
                        </span>
                      </div>
                    </td>
                    <td class="px-5 py-3.5">
                      <span class="text-xs" [class]="agent.twoFactorEnabled ? 'text-emerald-600 dark:text-emerald-400' : 'text-slate-400'">
                        {{ agent.twoFactorEnabled ? '✓ Ativo' : '— Inativo' }}
                      </span>
                    </td>
                    <td class="px-5 py-3.5 text-xs text-slate-500 dark:text-slate-400">
                      {{ formatDate(agent.createdAt) }}
                    </td>
                  </tr>
                }
              </tbody>
            </table>
          }
        </div>

      </div>
    </div>
  `,
})
export class AgentsPageComponent implements OnInit {
  private svc = inject(AgentService);
  private fb  = inject(FormBuilder);

  agents   = signal<AgentResponse[]>([]);
  loading  = signal(true);
  saving   = signal(false);
  showForm = signal(false);

  form = this.fb.group({
    name:     ['', Validators.required],
    email:    ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]],
    role:     ['Operator', Validators.required],
  });

  ngOnInit(): void {
    this.svc.list().subscribe({
      next:  a  => { this.agents.set(a); this.loading.set(false); },
      error: () => this.loading.set(false),
    });
  }

  create(): void {
    if (this.form.invalid) return;
    this.saving.set(true);
    const { name, email, password, role } = this.form.value;
    this.svc.create({ name: name!, email: email!, password: password!, role: role! }).subscribe({
      next: a => {
        this.agents.update(list => [...list, a]);
        this.cancelForm();
        this.saving.set(false);
      },
      error: () => this.saving.set(false),
    });
  }

  cancelForm(): void {
    this.form.reset({ role: 'Operator' });
    this.showForm.set(false);
  }

  initials(name: string): string {
    return name.split(' ').slice(0, 2).map(n => n[0]).join('').toUpperCase();
  }

  roleLabel(role: string): string {
    const map: Record<string, string> = { Operator: 'Operador', Supervisor: 'Supervisor', Admin: 'Admin' };
    return map[role] ?? role;
  }

  roleChip(role: string): string {
    const map: Record<string, string> = {
      Operator:   'bg-slate-100 dark:bg-slate-800 text-slate-600 dark:text-slate-400',
      Supervisor: 'bg-amber-50 dark:bg-amber-950 text-amber-700 dark:text-amber-300',
      Admin:      'bg-rose-50 dark:bg-rose-950 text-rose-700 dark:text-rose-300',
    };
    return map[role] ?? '';
  }

  formatDate(iso: string): string {
    return new Date(iso).toLocaleDateString('pt-BR');
  }
}
