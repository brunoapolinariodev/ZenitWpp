import { Component, inject, signal, computed, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ContactService } from '../../../core/services/contact.service';
import { ContactResponse } from '../../../core/models/contact.model';

@Component({
  selector: 'app-contacts',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule],
  template: `
    <div class="flex-1 overflow-y-auto bg-slate-50 dark:bg-slate-900/40">
      <div class="max-w-4xl mx-auto px-8 py-8">

        <div class="flex items-center justify-between mb-8">
          <div>
            <h1 class="text-xl font-semibold text-slate-900 dark:text-slate-50">Contatos</h1>
            <p class="text-sm text-slate-500 dark:text-slate-400 mt-1">{{ total() }} contatos cadastrados</p>
          </div>
          <button (click)="showForm.set(true)" class="btn-primary">+ Novo contato</button>
        </div>

        <!-- Busca -->
        <div class="relative mb-5">
          <svg class="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-slate-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0"/>
          </svg>
          <input [(ngModel)]="search" type="text" placeholder="Buscar por nome ou telefone..."
                 class="input pl-9 max-w-sm" />
        </div>

        <!-- Formulário -->
        @if (showForm()) {
          <div class="card p-6 mb-6">
            <h2 class="text-sm font-semibold text-slate-900 dark:text-slate-50 mb-4">Novo contato</h2>
            <form [formGroup]="form" (ngSubmit)="create()" class="grid grid-cols-2 gap-4">
              <div>
                <label class="block text-xs font-medium text-slate-600 dark:text-slate-400 mb-1.5">Nome</label>
                <input formControlName="name" class="input" placeholder="Nome completo" />
              </div>
              <div>
                <label class="block text-xs font-medium text-slate-600 dark:text-slate-400 mb-1.5">Telefone</label>
                <input formControlName="phone" class="input" placeholder="+55 11 99999-9999" />
              </div>
              <div>
                <label class="block text-xs font-medium text-slate-600 dark:text-slate-400 mb-1.5">E-mail</label>
                <input formControlName="email" type="email" class="input" placeholder="contato@email.com" />
              </div>
              <div>
                <label class="block text-xs font-medium text-slate-600 dark:text-slate-400 mb-1.5">Segmento</label>
                <input formControlName="segment" class="input" placeholder="Ex: Premium, VIP..." />
              </div>
              <div class="col-span-2 flex gap-2 justify-end pt-2">
                <button type="button" (click)="cancelForm()" class="btn-ghost">Cancelar</button>
                <button type="submit" [disabled]="form.invalid || saving()" class="btn-primary">
                  {{ saving() ? 'Salvando...' : 'Criar contato' }}
                </button>
              </div>
            </form>
          </div>
        }

        <!-- Lista -->
        <div class="card overflow-hidden">
          @if (loading()) {
            <div class="divide-y divide-slate-100 dark:divide-slate-800">
              @for (i of [1,2,3,4,5]; track i) {
                <div class="flex items-center gap-4 px-5 py-4 animate-pulse">
                  <div class="w-9 h-9 rounded-full bg-slate-200 dark:bg-slate-700"></div>
                  <div class="flex-1 space-y-2">
                    <div class="h-3 bg-slate-200 dark:bg-slate-700 rounded w-1/4"></div>
                    <div class="h-2.5 bg-slate-100 dark:bg-slate-800 rounded w-1/3"></div>
                  </div>
                </div>
              }
            </div>
          } @else if (filtered().length === 0) {
            <div class="flex flex-col items-center justify-center py-16 text-slate-400 dark:text-slate-500">
              <p class="text-sm">Nenhum contato encontrado</p>
            </div>
          } @else {
            <table class="w-full text-sm">
              <thead>
                <tr class="border-b border-slate-100 dark:border-slate-800">
                  <th class="text-left px-5 py-3 text-xs font-semibold text-slate-500 dark:text-slate-400">Contato</th>
                  <th class="text-left px-5 py-3 text-xs font-semibold text-slate-500 dark:text-slate-400">Telefone</th>
                  <th class="text-left px-5 py-3 text-xs font-semibold text-slate-500 dark:text-slate-400">Segmento</th>
                  <th class="text-left px-5 py-3 text-xs font-semibold text-slate-500 dark:text-slate-400">Idioma</th>
                  <th class="text-left px-5 py-3 text-xs font-semibold text-slate-500 dark:text-slate-400">Cadastro</th>
                </tr>
              </thead>
              <tbody class="divide-y divide-slate-100 dark:divide-slate-800">
                @for (c of filtered(); track c.id) {
                  <tr class="hover:bg-slate-50 dark:hover:bg-slate-800/40 transition-colors duration-100">
                    <td class="px-5 py-3.5">
                      <div class="flex items-center gap-3">
                        <div class="w-8 h-8 rounded-full flex items-center justify-center text-xs font-semibold
                                    bg-emerald-50 dark:bg-emerald-950 text-emerald-700 dark:text-emerald-300">
                          {{ initials(c.name) }}
                        </div>
                        <div>
                          <p class="font-medium text-slate-900 dark:text-slate-50">{{ c.name }}</p>
                          @if (c.email) {
                            <p class="text-xs text-slate-500 dark:text-slate-400">{{ c.email }}</p>
                          }
                        </div>
                      </div>
                    </td>
                    <td class="px-5 py-3.5 text-xs text-slate-600 dark:text-slate-400">{{ c.phone }}</td>
                    <td class="px-5 py-3.5">
                      @if (c.segment) {
                        <span class="px-2 py-0.5 rounded-full text-xs font-medium
                                     bg-slate-100 dark:bg-slate-800 text-slate-600 dark:text-slate-400">
                          {{ c.segment }}
                        </span>
                      } @else {
                        <span class="text-xs text-slate-400">—</span>
                      }
                    </td>
                    <td class="px-5 py-3.5 text-xs text-slate-600 dark:text-slate-400">{{ c.language || '—' }}</td>
                    <td class="px-5 py-3.5 text-xs text-slate-500 dark:text-slate-400">{{ formatDate(c.createdAt) }}</td>
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
export class ContactsPageComponent implements OnInit {
  private svc = inject(ContactService);
  private fb  = inject(FormBuilder);

  contacts = signal<ContactResponse[]>([]);
  loading  = signal(true);
  saving   = signal(false);
  showForm = signal(false);
  search   = '';

  total    = computed(() => this.contacts().length);
  filtered = computed(() => {
    const t = this.search.toLowerCase();
    return this.contacts().filter(c =>
      c.name.toLowerCase().includes(t) || c.phone.includes(t)
    );
  });

  form = this.fb.group({
    name:    ['', Validators.required],
    phone:   ['', Validators.required],
    email:   [''],
    segment: [''],
  });

  ngOnInit(): void {
    this.svc.list().subscribe({
      next:  c  => { this.contacts.set(c); this.loading.set(false); },
      error: () => this.loading.set(false),
    });
  }

  create(): void {
    if (this.form.invalid) return;
    this.saving.set(true);
    const { name, phone, email, segment } = this.form.value;
    this.svc.create({ name: name!, phone: phone!, email: email || undefined, segment: segment || undefined }).subscribe({
      next: c => {
        this.contacts.update(list => [...list, c]);
        this.cancelForm();
        this.saving.set(false);
      },
      error: () => this.saving.set(false),
    });
  }

  cancelForm(): void {
    this.form.reset();
    this.showForm.set(false);
  }

  initials(name: string): string {
    return name.split(' ').slice(0, 2).map(n => n[0]).join('').toUpperCase();
  }

  formatDate(iso: string): string {
    return new Date(iso).toLocaleDateString('pt-BR');
  }
}
