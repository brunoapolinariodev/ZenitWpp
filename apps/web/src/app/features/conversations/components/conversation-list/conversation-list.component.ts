import { Component, inject, output, signal, computed, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ConversationService } from '../../../../core/services/conversation.service';
import { ConversationResponse, ConversationStatus } from '../../../../core/models/conversation.model';

type FilterTab = { label: string; value: ConversationStatus | 'All' };

@Component({
  selector: 'app-conversation-list',
  standalone: true,
  imports: [FormsModule],
  template: `
    <div class="flex flex-col w-80 h-full border-r border-slate-100 dark:border-slate-800 bg-white dark:bg-slate-900 shrink-0">

      <!-- Header -->
      <div class="px-4 pt-5 pb-3 border-b border-slate-100 dark:border-slate-800">
        <h2 class="text-sm font-semibold text-slate-900 dark:text-slate-50 mb-3">
          Conversas
          @if (total() > 0) {
            <span class="ml-1.5 text-xs font-medium text-slate-400 dark:text-slate-500">
              {{ total() }}
            </span>
          }
        </h2>

        <!-- Busca -->
        <div class="relative">
          <svg class="absolute left-2.5 top-1/2 -translate-y-1/2 w-3.5 h-3.5 text-slate-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0"/>
          </svg>
          <input [(ngModel)]="search"
                 type="text"
                 placeholder="Buscar conversa..."
                 class="w-full pl-8 pr-3 py-1.5 text-xs rounded-lg
                        bg-slate-50 dark:bg-slate-800
                        border border-slate-200 dark:border-slate-700
                        text-slate-700 dark:text-slate-300
                        placeholder-slate-400 dark:placeholder-slate-500
                        focus:outline-none focus:border-primary-500 dark:focus:border-primary-400
                        transition duration-150" />
        </div>

        <!-- Filtros -->
        <div class="flex gap-1 mt-3 overflow-x-auto scrollbar-none">
          @for (tab of tabs; track tab.value) {
            <button (click)="setFilter(tab.value)"
                    class="shrink-0 px-2.5 py-1 text-xs rounded-md font-medium transition-colors duration-150"
                    [class]="activeFilter() === tab.value
                      ? 'bg-primary-50 dark:bg-primary-950 text-primary-600 dark:text-primary-400'
                      : 'text-slate-500 dark:text-slate-400 hover:bg-slate-100 dark:hover:bg-slate-800'">
              {{ tab.label }}
            </button>
          }
        </div>
      </div>

      <!-- Lista -->
      <div class="flex-1 overflow-y-auto">
        @if (loading()) {
          <div class="flex flex-col gap-1 p-2">
            @for (i of [1,2,3,4,5]; track i) {
              <div class="flex gap-3 p-3 rounded-lg animate-pulse">
                <div class="w-9 h-9 rounded-full bg-slate-200 dark:bg-slate-700 shrink-0"></div>
                <div class="flex-1 space-y-2 pt-0.5">
                  <div class="h-3 bg-slate-200 dark:bg-slate-700 rounded w-3/4"></div>
                  <div class="h-2.5 bg-slate-100 dark:bg-slate-800 rounded w-full"></div>
                </div>
              </div>
            }
          </div>
        } @else if (filtered().length === 0) {
          <div class="flex flex-col items-center justify-center h-48 text-slate-400 dark:text-slate-500">
            <svg class="w-8 h-8 mb-2 opacity-50" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M8 12h.01M12 12h.01M16 12h.01M21 12c0 4.418-4.03 8-9 8a9.863 9.863 0 01-4.255-.949L3 20l1.395-3.72C3.512 15.042 3 13.574 3 12c0-4.418 4.03-8 9-8s9 3.582 9 8z"/>
            </svg>
            <p class="text-xs">Nenhuma conversa</p>
          </div>
        } @else {
          <div class="p-2 flex flex-col gap-0.5">
            @for (conv of filtered(); track conv.id) {
              <button (click)="select(conv)"
                      class="w-full flex gap-3 px-3 py-2.5 rounded-lg text-left
                             hover:bg-slate-50 dark:hover:bg-slate-800/60
                             transition-colors duration-100 group"
                      [class.bg-primary-50]="selectedId() === conv.id"
                      [class.dark:bg-primary-950]="selectedId() === conv.id">

                <!-- Avatar -->
                <div class="relative shrink-0">
                  <div class="w-9 h-9 rounded-full flex items-center justify-center text-xs font-semibold"
                       [class]="avatarColors(conv.contactName)">
                    {{ initials(conv.contactName) }}
                  </div>
                  <span class="absolute -bottom-0.5 -right-0.5 w-2.5 h-2.5 rounded-full border-2 border-white dark:border-slate-900"
                        [class]="statusDot(conv.status)">
                  </span>
                </div>

                <!-- Conteúdo -->
                <div class="flex-1 min-w-0">
                  <div class="flex items-baseline justify-between gap-1">
                    <span class="text-xs font-medium text-slate-900 dark:text-slate-100 truncate">
                      {{ conv.contactName }}
                    </span>
                    <span class="text-[10px] text-slate-400 dark:text-slate-500 shrink-0">
                      {{ formatTime(conv.lastMessageAt) }}
                    </span>
                  </div>
                  <div class="flex items-center justify-between gap-1 mt-0.5">
                    <p class="text-[11px] text-slate-500 dark:text-slate-400 truncate leading-relaxed">
                      {{ conv.lastMessagePreview }}
                    </p>
                    @if (conv.unreadCount > 0) {
                      <span class="shrink-0 min-w-[18px] h-[18px] px-1 rounded-full text-[10px] font-semibold
                                   bg-primary-600 text-white flex items-center justify-center">
                        {{ conv.unreadCount > 99 ? '99+' : conv.unreadCount }}
                      </span>
                    }
                  </div>
                </div>

              </button>
            }
          </div>
        }
      </div>
    </div>
  `,
})
export class ConversationListComponent implements OnInit {
  private svc = inject(ConversationService);

  selected = output<ConversationResponse>();

  conversations = signal<ConversationResponse[]>([]);
  loading       = signal(true);
  total         = signal(0);
  selectedId    = signal<string | null>(null);
  search        = '';
  activeFilter  = signal<ConversationStatus | 'All'>('All');

  tabs: FilterTab[] = [
    { label: 'Todas',       value: 'All' },
    { label: 'Abertas',     value: 'Open' },
    { label: 'Em andamento',value: 'InProgress' },
    { label: 'Aguardando',  value: 'Waiting' },
    { label: 'Encerradas',  value: 'Closed' },
  ];

  filtered = computed(() => {
    const term = this.search.toLowerCase();
    return this.conversations().filter(c =>
      c.contactName.toLowerCase().includes(term) ||
      c.lastMessagePreview.toLowerCase().includes(term)
    );
  });

  ngOnInit(): void {
    this.load();
  }

  setFilter(value: ConversationStatus | 'All'): void {
    this.activeFilter.set(value);
    this.load();
  }

  select(conv: ConversationResponse): void {
    this.selectedId.set(conv.id);
    this.selected.emit(conv);
  }

  private load(): void {
    this.loading.set(true);
    const status = this.activeFilter() === 'All' ? undefined : this.activeFilter() as ConversationStatus;
    this.svc.list({ status, pageSize: 50 }).subscribe({
      next: res => {
        this.conversations.set(res.items);
        this.total.set(res.total);
        this.loading.set(false);
      },
      error: () => this.loading.set(false),
    });
  }

  initials(name: string): string {
    return name.split(' ').slice(0, 2).map(n => n[0]).join('').toUpperCase();
  }

  avatarColors(name: string): string {
    const palettes = [
      'bg-violet-100 dark:bg-violet-900/40 text-violet-700 dark:text-violet-300',
      'bg-blue-100 dark:bg-blue-900/40 text-blue-700 dark:text-blue-300',
      'bg-emerald-100 dark:bg-emerald-900/40 text-emerald-700 dark:text-emerald-300',
      'bg-amber-100 dark:bg-amber-900/40 text-amber-700 dark:text-amber-300',
      'bg-rose-100 dark:bg-rose-900/40 text-rose-700 dark:text-rose-300',
      'bg-cyan-100 dark:bg-cyan-900/40 text-cyan-700 dark:text-cyan-300',
    ];
    const i = name.charCodeAt(0) % palettes.length;
    return palettes[i];
  }

  statusDot(status: ConversationStatus): string {
    const map: Record<ConversationStatus, string> = {
      Open:       'bg-emerald-400',
      InProgress: 'bg-primary-500',
      Waiting:    'bg-amber-400',
      Closed:     'bg-slate-300 dark:bg-slate-600',
    };
    return map[status];
  }

  formatTime(iso: string): string {
    const d = new Date(iso);
    const now = new Date();
    const isToday = d.toDateString() === now.toDateString();
    if (isToday) return d.toLocaleTimeString('pt-BR', { hour: '2-digit', minute: '2-digit' });
    const diff = Math.floor((now.getTime() - d.getTime()) / 86400000);
    if (diff < 7) return d.toLocaleDateString('pt-BR', { weekday: 'short' });
    return d.toLocaleDateString('pt-BR', { day: '2-digit', month: '2-digit' });
  }
}
