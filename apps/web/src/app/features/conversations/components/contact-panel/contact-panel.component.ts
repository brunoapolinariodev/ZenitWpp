import { Component, input, computed } from '@angular/core';
import { ConversationResponse } from '../../../../core/models/conversation.model';

@Component({
  selector: 'app-contact-panel',
  standalone: true,
  template: `
    <div class="flex flex-col w-72 h-full shrink-0
                bg-white dark:bg-slate-900
                border-l border-slate-100 dark:border-slate-800
                overflow-y-auto">

      @if (!conversation()) {
        <div class="flex items-center justify-center h-full text-slate-400 dark:text-slate-500">
          <p class="text-xs">Nenhum contato selecionado</p>
        </div>
      } @else {
        <!-- Avatar e nome -->
        <div class="flex flex-col items-center px-5 pt-7 pb-5 border-b border-slate-100 dark:border-slate-800">
          <div class="w-14 h-14 rounded-full flex items-center justify-center
                      text-lg font-semibold mb-3"
               [class]="avatarColors(conversation()!.contactName)">
            {{ initials(conversation()!.contactName) }}
          </div>
          <p class="text-sm font-semibold text-slate-900 dark:text-slate-50 text-center">
            {{ conversation()!.contactName }}
          </p>
          <p class="text-xs text-slate-500 dark:text-slate-400 mt-0.5">
            {{ conversation()!.contactPhone }}
          </p>
        </div>

        <!-- Info -->
        <div class="px-4 py-4 space-y-4">

          <!-- Status da conversa -->
          <div>
            <p class="text-[10px] font-semibold text-slate-400 dark:text-slate-500 uppercase tracking-wider mb-2">
              Conversa
            </p>
            <div class="space-y-2">
              <div class="flex items-center justify-between">
                <span class="text-xs text-slate-500 dark:text-slate-400">Status</span>
                <span class="text-xs font-medium px-2 py-0.5 rounded-full"
                      [class]="statusChip(conversation()!.status)">
                  {{ statusLabel(conversation()!.status) }}
                </span>
              </div>
              @if (conversation()!.assignedAgentName) {
                <div class="flex items-center justify-between">
                  <span class="text-xs text-slate-500 dark:text-slate-400">Agente</span>
                  <span class="text-xs font-medium text-slate-700 dark:text-slate-300">
                    {{ conversation()!.assignedAgentName }}
                  </span>
                </div>
              }
              <div class="flex items-center justify-between">
                <span class="text-xs text-slate-500 dark:text-slate-400">Início</span>
                <span class="text-xs text-slate-700 dark:text-slate-300">
                  {{ formatDate(conversation()!.createdAt) }}
                </span>
              </div>
              <div class="flex items-center justify-between">
                <span class="text-xs text-slate-500 dark:text-slate-400">Mensagens</span>
                <span class="text-xs font-medium text-slate-700 dark:text-slate-300">
                  {{ conversation()!.messages?.length ?? 0 }}
                </span>
              </div>
            </div>
          </div>

          <div class="h-px bg-slate-100 dark:bg-slate-800"></div>

          <!-- Canal -->
          <div>
            <p class="text-[10px] font-semibold text-slate-400 dark:text-slate-500 uppercase tracking-wider mb-2">
              Canal
            </p>
            <div class="flex items-center gap-2">
              <span class="text-base">📱</span>
              <span class="text-xs font-medium text-slate-700 dark:text-slate-300">
                {{ conversation()!.channel || 'WhatsApp' }}
              </span>
            </div>
          </div>

          <div class="h-px bg-slate-100 dark:bg-slate-800"></div>

          <!-- Ações rápidas -->
          <div>
            <p class="text-[10px] font-semibold text-slate-400 dark:text-slate-500 uppercase tracking-wider mb-2">
              Ações
            </p>
            <div class="flex flex-col gap-1.5">
              <button class="flex items-center gap-2.5 px-3 py-2 rounded-lg text-xs font-medium
                             text-slate-600 dark:text-slate-300
                             hover:bg-slate-50 dark:hover:bg-slate-800
                             transition-colors duration-150 text-left">
                <svg class="w-3.5 h-3.5 text-slate-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                        d="M8 7h12m0 0l-4-4m4 4l-4 4m0 6H4m0 0l4 4m-4-4l4-4"/>
                </svg>
                Transferir conversa
              </button>
              <button class="flex items-center gap-2.5 px-3 py-2 rounded-lg text-xs font-medium
                             text-slate-600 dark:text-slate-300
                             hover:bg-slate-50 dark:hover:bg-slate-800
                             transition-colors duration-150 text-left">
                <svg class="w-3.5 h-3.5 text-slate-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                        d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"/>
                </svg>
                Ver perfil do contato
              </button>
            </div>
          </div>

        </div>
      }
    </div>
  `,
})
export class ContactPanelComponent {
  conversation = input<ConversationResponse | null>(null);

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
    ];
    return palettes[name.charCodeAt(0) % palettes.length];
  }

  statusChip(status: string): string {
    const map: Record<string, string> = {
      Open:       'bg-emerald-50 dark:bg-emerald-950 text-emerald-700 dark:text-emerald-300',
      InProgress: 'bg-primary-50 dark:bg-primary-950 text-primary-700 dark:text-primary-300',
      Waiting:    'bg-amber-50 dark:bg-amber-950 text-amber-700 dark:text-amber-300',
      Closed:     'bg-slate-100 dark:bg-slate-800 text-slate-600 dark:text-slate-400',
    };
    return map[status] ?? '';
  }

  statusLabel(status: string): string {
    const map: Record<string, string> = {
      Open: 'Aberta', InProgress: 'Em andamento', Waiting: 'Aguardando', Closed: 'Encerrada',
    };
    return map[status] ?? status;
  }

  formatDate(iso: string): string {
    return new Date(iso).toLocaleDateString('pt-BR', { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit' });
  }
}
