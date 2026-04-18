import { Component, inject, signal, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { AutomationService, FlowResponse, TriggerType } from '../../../core/services/automation.service';

@Component({
  selector: 'app-automation',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule],
  template: `
    <div class="flex-1 overflow-y-auto bg-slate-50 dark:bg-slate-900/40">
      <div class="max-w-4xl mx-auto px-8 py-8">

        <div class="flex items-center justify-between mb-8">
          <div>
            <h1 class="text-xl font-semibold text-slate-900 dark:text-slate-50">Automação</h1>
            <p class="text-sm text-slate-500 dark:text-slate-400 mt-1">Configure fluxos automáticos de atendimento</p>
          </div>
          <button (click)="showForm.set(true)" class="btn-primary">+ Novo fluxo</button>
        </div>

        <!-- Formulário -->
        @if (showForm()) {
          <div class="card p-6 mb-6">
            <h2 class="text-sm font-semibold text-slate-900 dark:text-slate-50 mb-4">Novo fluxo</h2>
            <form [formGroup]="form" (ngSubmit)="create()" class="grid grid-cols-2 gap-4">
              <div>
                <label class="block text-xs font-medium text-slate-600 dark:text-slate-400 mb-1.5">Nome do fluxo</label>
                <input formControlName="name" class="input" placeholder="Ex: Boas-vindas" />
              </div>
              <div>
                <label class="block text-xs font-medium text-slate-600 dark:text-slate-400 mb-1.5">Tipo de gatilho</label>
                <select formControlName="triggerType" class="input">
                  <option value="Keyword">Palavra-chave</option>
                  <option value="FirstMessage">Primeira mensagem</option>
                  <option value="OutsideHours">Fora do horário</option>
                </select>
              </div>
              @if (form.value.triggerType === 'Keyword') {
                <div class="col-span-2">
                  <label class="block text-xs font-medium text-slate-600 dark:text-slate-400 mb-1.5">Palavra-chave</label>
                  <input formControlName="triggerValue" class="input" placeholder="Ex: oi, olá, menu..." />
                </div>
              }
              <div class="col-span-2 flex gap-2 justify-end pt-2">
                <button type="button" (click)="cancelForm()" class="btn-ghost">Cancelar</button>
                <button type="submit" [disabled]="form.invalid || saving()" class="btn-primary">
                  {{ saving() ? 'Salvando...' : 'Criar fluxo' }}
                </button>
              </div>
            </form>
          </div>
        }

        <!-- Lista -->
        <div class="card overflow-hidden">
          @if (loading()) {
            <div class="divide-y divide-slate-100 dark:divide-slate-800">
              @for (i of [1,2,3]; track i) {
                <div class="flex items-center gap-4 px-5 py-4 animate-pulse">
                  <div class="w-8 h-8 rounded-lg bg-slate-200 dark:bg-slate-700"></div>
                  <div class="flex-1 space-y-2">
                    <div class="h-3 bg-slate-200 dark:bg-slate-700 rounded w-1/4"></div>
                    <div class="h-2.5 bg-slate-100 dark:bg-slate-800 rounded w-1/3"></div>
                  </div>
                </div>
              }
            </div>
          } @else if (flows().length === 0) {
            <div class="flex flex-col items-center justify-center py-16 text-slate-400 dark:text-slate-500">
              <p class="text-sm">Nenhum fluxo configurado</p>
              <p class="text-xs mt-1">Crie um fluxo para automatizar respostas</p>
            </div>
          } @else {
            <table class="w-full text-sm">
              <thead>
                <tr class="border-b border-slate-100 dark:border-slate-800">
                  <th class="text-left px-5 py-3 text-xs font-semibold text-slate-500 dark:text-slate-400">Fluxo</th>
                  <th class="text-left px-5 py-3 text-xs font-semibold text-slate-500 dark:text-slate-400">Gatilho</th>
                  <th class="text-left px-5 py-3 text-xs font-semibold text-slate-500 dark:text-slate-400">Valor</th>
                  <th class="text-center px-5 py-3 text-xs font-semibold text-slate-500 dark:text-slate-400">Passos</th>
                  <th class="text-left px-5 py-3 text-xs font-semibold text-slate-500 dark:text-slate-400">Status</th>
                </tr>
              </thead>
              <tbody class="divide-y divide-slate-100 dark:divide-slate-800">
                @for (f of flows(); track f.id) {
                  <tr class="hover:bg-slate-50 dark:hover:bg-slate-800/40 transition-colors duration-100">
                    <td class="px-5 py-3.5 font-medium text-slate-900 dark:text-slate-50">{{ f.name }}</td>
                    <td class="px-5 py-3.5">
                      <span class="px-2 py-0.5 rounded-full text-xs font-medium
                                   bg-primary-50 dark:bg-primary-950 text-primary-700 dark:text-primary-300">
                        {{ triggerLabel(f.triggerType) }}
                      </span>
                    </td>
                    <td class="px-5 py-3.5 text-xs text-slate-600 dark:text-slate-400">{{ f.triggerValue || '—' }}</td>
                    <td class="px-5 py-3.5 text-xs text-center text-slate-600 dark:text-slate-400">{{ f.stepsCount }}</td>
                    <td class="px-5 py-3.5">
                      <span class="flex items-center gap-1.5 text-xs"
                            [class]="f.isActive ? 'text-emerald-600 dark:text-emerald-400' : 'text-slate-400'">
                        <span class="w-1.5 h-1.5 rounded-full"
                              [class]="f.isActive ? 'bg-emerald-400' : 'bg-slate-300 dark:bg-slate-600'">
                        </span>
                        {{ f.isActive ? 'Ativo' : 'Inativo' }}
                      </span>
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
export class AutomationPageComponent implements OnInit {
  private svc = inject(AutomationService);
  private fb  = inject(FormBuilder);

  flows   = signal<FlowResponse[]>([]);
  loading = signal(true);
  saving  = signal(false);
  showForm = signal(false);

  form = this.fb.group({
    name:         ['', Validators.required],
    triggerType:  ['Keyword', Validators.required],
    triggerValue: [''],
  });

  ngOnInit(): void {
    this.svc.list().subscribe({
      next:  f  => { this.flows.set(f); this.loading.set(false); },
      error: () => this.loading.set(false),
    });
  }

  create(): void {
    if (this.form.invalid) return;
    this.saving.set(true);
    const { name, triggerType, triggerValue } = this.form.value;
    this.svc.create({ name: name!, triggerType: triggerType as TriggerType, triggerValue: triggerValue || undefined }).subscribe({
      next: f => {
        this.flows.update(list => [f, ...list]);
        this.cancelForm();
        this.saving.set(false);
      },
      error: () => this.saving.set(false),
    });
  }

  cancelForm(): void {
    this.form.reset({ triggerType: 'Keyword' });
    this.showForm.set(false);
  }

  triggerLabel(t: TriggerType): string {
    const map: Record<TriggerType, string> = {
      Keyword: 'Palavra-chave', FirstMessage: 'Primeira msg', OutsideHours: 'Fora do horário',
    };
    return map[t];
  }
}
