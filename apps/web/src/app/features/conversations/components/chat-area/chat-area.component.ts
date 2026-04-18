import {
  Component, inject, input, output, signal, effect,
  ViewChild, ElementRef, OnDestroy
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Subscription } from 'rxjs';
import { ConversationService } from '../../../../core/services/conversation.service';
import { SignalRService } from '../../../../core/services/signalr.service';
import { ConversationResponse, MessageResponse, ConversationStatus } from '../../../../core/models/conversation.model';

@Component({
  selector: 'app-chat-area',
  standalone: true,
  imports: [FormsModule],
  template: `
    @if (!conversation()) {
      <!-- Estado vazio -->
      <div class="flex flex-1 flex-col items-center justify-center gap-3
                  text-slate-400 dark:text-slate-500 bg-slate-50 dark:bg-slate-900/50">
        <svg class="w-12 h-12 opacity-30" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5"
                d="M8 12h.01M12 12h.01M16 12h.01M21 12c0 4.418-4.03 8-9 8a9.863 9.863 0 01-4.255-.949L3 20l1.395-3.72C3.512 15.042 3 13.574 3 12c0-4.418 4.03-8 9-8s9 3.582 9 8z"/>
        </svg>
        <p class="text-sm">Selecione uma conversa</p>
      </div>
    } @else {
      <div class="flex flex-1 flex-col min-w-0 bg-slate-50 dark:bg-slate-900/40">

        <!-- Header -->
        <div class="flex items-center gap-3 px-5 py-3.5
                    bg-white dark:bg-slate-900
                    border-b border-slate-100 dark:border-slate-800 shrink-0">
          <div class="flex-1 min-w-0">
            <p class="text-sm font-semibold text-slate-900 dark:text-slate-50 truncate">
              {{ conversation()!.contactName }}
            </p>
            <p class="text-xs text-slate-500 dark:text-slate-400 mt-0.5">
              {{ conversation()!.contactPhone }}
              <span class="mx-1.5 text-slate-300 dark:text-slate-600">·</span>
              <span [class]="statusBadge(conversation()!.status)">
                {{ statusLabel(conversation()!.status) }}
              </span>
            </p>
          </div>

          <!-- Ações -->
          <div class="flex items-center gap-1">
            @if (conversation()!.status !== 'Closed') {
              <button (click)="closeConversation()"
                      title="Encerrar conversa"
                      class="flex items-center gap-1.5 px-3 py-1.5 rounded-lg text-xs font-medium
                             text-slate-600 dark:text-slate-300
                             hover:bg-red-50 dark:hover:bg-red-950
                             hover:text-red-600 dark:hover:text-red-400
                             transition-colors duration-150">
                <svg class="w-3.5 h-3.5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12"/>
                </svg>
                Encerrar
              </button>
            }
          </div>
        </div>

        <!-- Mensagens -->
        <div #messagesContainer
             class="flex-1 overflow-y-auto px-5 py-4 space-y-3">

          @if (messages().length === 0) {
            <div class="flex items-center justify-center h-full text-slate-400 dark:text-slate-500">
              <p class="text-xs">Nenhuma mensagem ainda</p>
            </div>
          }

          @for (msg of messages(); track msg.id; let i = $index) {
            <!-- Separador de data -->
            @if (showDateSeparator(msg, i)) {
              <div class="flex items-center gap-3 py-1">
                <div class="flex-1 h-px bg-slate-200 dark:bg-slate-700"></div>
                <span class="text-[10px] font-medium text-slate-400 dark:text-slate-500 shrink-0">
                  {{ formatDateSeparator(msg.sentAt) }}
                </span>
                <div class="flex-1 h-px bg-slate-200 dark:bg-slate-700"></div>
              </div>
            }

            <!-- Bubble -->
            <div class="flex"
                 [class.justify-end]="msg.direction === 'Outbound'"
                 [class.justify-start]="msg.direction === 'Inbound'">
              <div class="max-w-[72%]">
                @if (msg.direction === 'Inbound' && msg.senderName) {
                  <p class="text-[10px] font-medium text-slate-500 dark:text-slate-400 mb-1 ml-1">
                    {{ msg.senderName }}
                  </p>
                }
                <div class="px-3.5 py-2.5 rounded-2xl text-sm leading-relaxed"
                     [class]="bubbleClass(msg.direction)">
                  {{ msg.content }}
                </div>
                <p class="text-[10px] text-slate-400 dark:text-slate-500 mt-1"
                   [class.text-right]="msg.direction === 'Outbound'"
                   [class.ml-1]="msg.direction === 'Inbound'">
                  {{ formatTime(msg.sentAt) }}
                </p>
              </div>
            </div>
          }

          <!-- Indicador de digitação -->
          @if (someoneTyping()) {
            <div class="flex justify-start">
              <div class="px-4 py-2.5 rounded-2xl bg-white dark:bg-slate-800 border border-slate-100 dark:border-slate-700">
                <div class="flex gap-1 items-center">
                  <div class="w-1.5 h-1.5 rounded-full bg-slate-400 animate-bounce [animation-delay:0ms]"></div>
                  <div class="w-1.5 h-1.5 rounded-full bg-slate-400 animate-bounce [animation-delay:150ms]"></div>
                  <div class="w-1.5 h-1.5 rounded-full bg-slate-400 animate-bounce [animation-delay:300ms]"></div>
                </div>
              </div>
            </div>
          }
        </div>

        <!-- Input -->
        @if (conversation()!.status !== 'Closed') {
          <div class="px-4 py-3 bg-white dark:bg-slate-900 border-t border-slate-100 dark:border-slate-800 shrink-0">
            <div class="flex items-end gap-2">
              <div class="flex-1 relative">
                <textarea [(ngModel)]="messageText"
                          (keydown.enter)="onEnter($event)"
                          (input)="onTyping()"
                          placeholder="Digite uma mensagem..."
                          rows="1"
                          class="w-full px-3.5 py-2.5 pr-10 text-sm rounded-xl resize-none
                                 bg-slate-50 dark:bg-slate-800
                                 border border-slate-200 dark:border-slate-700
                                 text-slate-900 dark:text-slate-50
                                 placeholder-slate-400 dark:placeholder-slate-500
                                 focus:outline-none focus:border-primary-500 dark:focus:border-primary-400
                                 focus:ring-2 focus:ring-primary-500/10
                                 transition duration-150 max-h-32 overflow-y-auto">
                </textarea>
              </div>
              <button (click)="send()"
                      [disabled]="!messageText.trim() || sending()"
                      class="shrink-0 w-9 h-9 rounded-xl flex items-center justify-center
                             bg-primary-600 text-white
                             hover:bg-primary-700 active:bg-primary-800
                             disabled:opacity-40 disabled:cursor-not-allowed
                             transition-colors duration-150">
                <svg class="w-4 h-4 rotate-90" fill="currentColor" viewBox="0 0 24 24">
                  <path d="M2.01 21L23 12 2.01 3 2 10l15 2-15 2z"/>
                </svg>
              </button>
            </div>
            <p class="text-[10px] text-slate-400 dark:text-slate-500 mt-1.5 ml-1">
              Enter para enviar · Shift+Enter para nova linha
            </p>
          </div>
        } @else {
          <div class="px-4 py-3 bg-white dark:bg-slate-900 border-t border-slate-100 dark:border-slate-800">
            <p class="text-xs text-center text-slate-400 dark:text-slate-500">
              Conversa encerrada
            </p>
          </div>
        }

      </div>
    }
  `,
})
export class ChatAreaComponent implements OnDestroy {
  private svc      = inject(ConversationService);
  private signalR  = inject(SignalRService);

  conversation = input<ConversationResponse | null>(null);
  closed       = output<void>();

  @ViewChild('messagesContainer') messagesContainer?: ElementRef<HTMLDivElement>;

  messages      = signal<MessageResponse[]>([]);
  messageText   = '';
  sending       = signal(false);
  someoneTyping = signal(false);

  private typingTimeout?: ReturnType<typeof setTimeout>;
  private subs: Subscription[] = [];

  constructor() {
    effect(() => {
      const conv = this.conversation();
      if (conv) {
        this.messages.set(conv.messages ?? []);
        this.joinHub(conv.id);
        setTimeout(() => this.scrollToBottom(), 50);
      }
    });

    this.subs.push(
      this.signalR.newMessage$.subscribe(ev => {
        if (ev.conversationId === this.conversation()?.id) {
          this.messages.update(msgs => [...msgs, ev.message]);
          setTimeout(() => this.scrollToBottom(), 50);
        }
      }),
      this.signalR.typing$.subscribe(ev => {
        if (ev.conversationId === this.conversation()?.id) {
          this.someoneTyping.set(true);
          clearTimeout(this.typingTimeout);
          this.typingTimeout = setTimeout(() => this.someoneTyping.set(false), 3000);
        }
      }),
    );
  }

  ngOnDestroy(): void {
    this.subs.forEach(s => s.unsubscribe());
  }

  onEnter(event: Event): void {
    const e = event as KeyboardEvent;
    if (!e.shiftKey) { e.preventDefault(); this.send(); }
  }

  onTyping(): void {
    const id = this.conversation()?.id;
    if (id) this.signalR.notifyTyping(id);
  }

  send(): void {
    const text = this.messageText.trim();
    const id   = this.conversation()?.id;
    if (!text || !id || this.sending()) return;

    this.sending.set(true);
    this.svc.sendMessage(id, text).subscribe({
      next: () => {
        this.messageText = '';
        this.sending.set(false);
      },
      error: () => this.sending.set(false),
    });
  }

  closeConversation(): void {
    const id = this.conversation()?.id;
    if (!id) return;
    this.svc.close(id).subscribe(() => this.closed.emit());
  }

  private async joinHub(id: string): Promise<void> {
    await this.signalR.connect();
    await this.signalR.joinConversation(id);
  }

  private scrollToBottom(): void {
    const el = this.messagesContainer?.nativeElement;
    if (el) el.scrollTop = el.scrollHeight;
  }

  bubbleClass(direction: string): string {
    return direction === 'Outbound'
      ? 'bg-primary-600 text-white rounded-br-sm'
      : 'bg-white dark:bg-slate-800 text-slate-900 dark:text-slate-50 border border-slate-100 dark:border-slate-700 rounded-bl-sm';
  }

  statusBadge(status: ConversationStatus): string {
    const map: Record<ConversationStatus, string> = {
      Open:       'text-emerald-600 dark:text-emerald-400',
      InProgress: 'text-primary-600 dark:text-primary-400',
      Waiting:    'text-amber-600 dark:text-amber-400',
      Closed:     'text-slate-500 dark:text-slate-400',
    };
    return map[status];
  }

  statusLabel(status: ConversationStatus): string {
    const map: Record<ConversationStatus, string> = {
      Open: 'Aberta', InProgress: 'Em andamento', Waiting: 'Aguardando', Closed: 'Encerrada',
    };
    return map[status];
  }

  formatTime(iso: string): string {
    return new Date(iso).toLocaleTimeString('pt-BR', { hour: '2-digit', minute: '2-digit' });
  }

  formatDateSeparator(iso: string): string {
    const d = new Date(iso);
    const now = new Date();
    if (d.toDateString() === now.toDateString()) return 'Hoje';
    const yesterday = new Date(now);
    yesterday.setDate(now.getDate() - 1);
    if (d.toDateString() === yesterday.toDateString()) return 'Ontem';
    return d.toLocaleDateString('pt-BR', { day: '2-digit', month: 'long', year: 'numeric' });
  }

  showDateSeparator(msg: MessageResponse, index: number): boolean {
    if (index === 0) return true;
    const prev = this.messages()[index - 1];
    return new Date(msg.sentAt).toDateString() !== new Date(prev.sentAt).toDateString();
  }
}
