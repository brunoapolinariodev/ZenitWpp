import { Component, inject, signal, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { CampaignService, CampaignResponse, CampaignStatus } from '../../../core/services/campaign.service';

@Component({
  selector: 'app-campaigns',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule],
  template: `
    <div class="flex-1 overflow-y-auto bg-slate-50 dark:bg-slate-900/40">
      <div class="max-w-4xl mx-auto px-8 py-8">

        <div class="flex items-center justify-between mb-8">
          <div>
            <h1 class="text-xl font-semibold text-slate-900 dark:text-slate-50">Campanhas</h1>
            <p class="text-sm text-slate-500 dark:text-slate-400 mt-1">Gerencie e dispare campanhas de mensagens</p>
          </div>
          <button (click)="showForm.set(true)" class="btn-primary">+ Nova campanha</button>
        </div>

        <!-- Formulário -->
        @if (showForm()) {
          <div class="card p-6 mb-6">
            <h2 class="text-sm font-semibold text-slate-900 dark:text-slate-50 mb-4">Nova campanha</h2>
            <form [formGroup]="form" (ngSubmit)="create()" class="space-y-4">
              <div class="grid grid-cols-2 gap-4">
                <div>
                  <label class="block text-xs font-medium text-slate-600 dark:text-slate-400 mb-1.5">Nome</label>
                  <input formControlName="name" class="input" placeholder="Ex: Promoção de Maio" />
                </div>
                <div>
                  <label class="block text-xs font-medium text-slate-600 dark:text-slate-400 mb-1.5">Segmento</label>
                  <input formControlName="segment" class="input" placeholder="Ex: Premium (opcional)" />
                </div>
              </div>
              <div>
                <label class="block text-xs font-medium text-slate-600 dark:text-slate-400 mb-1.5">Mensagem</label>
                <textarea formControlName="message" rows="3" class="input resize-none"
                          placeholder="Olá {nome}, temos uma novidade especial para você..."></textarea>
              </div>
              <div class="flex gap-2 justify-end pt-2">
                <button type="button" (click)="cancelForm()" class="btn-ghost">Cancelar</button>
                <button type="submit" [disabled]="form.invalid || saving()" class="btn-primary">
                  {{ saving() ? 'Salvando...' : 'Criar campanha' }}
                </button>
              </div>
            </form>
          </div>
        }

        <!-- Lista de campanhas (mock) -->
        <div class="card overflow-hidden">
          @if (campaigns().length === 0) {
            <div class="flex flex-col items-center justify-center py-16 text-slate-400 dark:text-slate-500">
              <p class="text-sm">Nenhuma campanha criada</p>
            </div>
          } @else {
            <table class="w-full text-sm">
              <thead>
                <tr class="border-b border-slate-100 dark:border-slate-800">
                  <th class="text-left px-5 py-3 text-xs font-semibold text-slate-500 dark:text-slate-400">Nome</th>
                  <th class="text-left px-5 py-3 text-xs font-semibold text-slate-500 dark:text-slate-400">Status</th>
                  <th class="text-left px-5 py-3 text-xs font-semibold text-slate-500 dark:text-slate-400">Segmento</th>
                  <th class="text-right px-5 py-3 text-xs font-semibold text-slate-500 dark:text-slate-400">Enviados</th>
                  <th class="text-right px-5 py-3 text-xs font-semibold text-slate-500 dark:text-slate-400">Falhas</th>
                  <th class="px-5 py-3"></th>
                </tr>
              </thead>
              <tbody class="divide-y divide-slate-100 dark:divide-slate-800">
                @for (c of campaigns(); track c.id) {
                  <tr class="hover:bg-slate-50 dark:hover:bg-slate-800/40 transition-colors duration-100">
                    <td class="px-5 py-3.5">
                      <p class="font-medium text-slate-900 dark:text-slate-50">{{ c.name }}</p>
                      <p class="text-xs text-slate-500 dark:text-slate-400 mt-0.5 truncate max-w-xs">{{ c.message }}</p>
                    </td>
                    <td class="px-5 py-3.5">
                      <span class="px-2 py-0.5 rounded-full text-xs font-medium" [class]="statusChip(c.status)">
                        {{ statusLabel(c.status) }}
                      </span>
                    </td>
                    <td class="px-5 py-3.5 text-xs text-slate-600 dark:text-slate-400">{{ c.segment || '—' }}</td>
                    <td class="px-5 py-3.5 text-xs text-right text-emerald-600 dark:text-emerald-400 font-medium">{{ c.totalSent }}</td>
                    <td class="px-5 py-3.5 text-xs text-right text-rose-600 dark:text-rose-400 font-medium">{{ c.totalFailed }}</td>
                    <td class="px-5 py-3.5 text-right">
                      @if (c.status === 'Draft') {
                        <button (click)="send(c)"
                                class="text-xs font-medium text-primary-600 dark:text-primary-400
                                       hover:text-primary-700 dark:hover:text-primary-300 transition-colors">
                          Disparar →
                        </button>
                      }
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
export class CampaignsPageComponent implements OnInit {
  private svc = inject(CampaignService);
  private fb  = inject(FormBuilder);

  campaigns = signal<CampaignResponse[]>([]);
  saving    = signal(false);
  showForm  = signal(false);

  form = this.fb.group({
    name:    ['', Validators.required],
    message: ['', Validators.required],
    segment: [''],
  });

  ngOnInit(): void {
    // API não retorna lista, inicia vazio
  }

  create(): void {
    if (this.form.invalid) return;
    this.saving.set(true);
    const { name, message, segment } = this.form.value;
    this.svc.create({ name: name!, message: message!, segment: segment || undefined }).subscribe({
      next: c => {
        this.campaigns.update(list => [c, ...list]);
        this.cancelForm();
        this.saving.set(false);
      },
      error: () => this.saving.set(false),
    });
  }

  send(c: CampaignResponse): void {
    this.svc.send(c.id).subscribe(() => {
      this.campaigns.update(list =>
        list.map(x => x.id === c.id ? { ...x, status: 'Running' as CampaignStatus } : x)
      );
    });
  }

  cancelForm(): void {
    this.form.reset();
    this.showForm.set(false);
  }

  statusLabel(s: CampaignStatus): string {
    const map: Record<CampaignStatus, string> = {
      Draft: 'Rascunho', Scheduled: 'Agendada', Running: 'Enviando', Completed: 'Concluída', Cancelled: 'Cancelada',
    };
    return map[s];
  }

  statusChip(s: CampaignStatus): string {
    const map: Record<CampaignStatus, string> = {
      Draft:     'bg-slate-100 dark:bg-slate-800 text-slate-600 dark:text-slate-400',
      Scheduled: 'bg-amber-50 dark:bg-amber-950 text-amber-700 dark:text-amber-300',
      Running:   'bg-primary-50 dark:bg-primary-950 text-primary-700 dark:text-primary-300',
      Completed: 'bg-emerald-50 dark:bg-emerald-950 text-emerald-700 dark:text-emerald-300',
      Cancelled: 'bg-rose-50 dark:bg-rose-950 text-rose-700 dark:text-rose-300',
    };
    return map[s];
  }
}
