import { Component, signal } from '@angular/core';
import { ConversationListComponent } from '../components/conversation-list/conversation-list.component';
import { ChatAreaComponent } from '../components/chat-area/chat-area.component';
import { ContactPanelComponent } from '../components/contact-panel/contact-panel.component';
import { ConversationResponse } from '../../../core/models/conversation.model';

@Component({
  selector: 'app-conversations-shell',
  standalone: true,
  imports: [ConversationListComponent, ChatAreaComponent, ContactPanelComponent],
  template: `
    <div class="flex flex-1 overflow-hidden">
      <app-conversation-list (selected)="onSelect($event)" />
      <app-chat-area [conversation]="active()" (closed)="onClosed()" />
      <app-contact-panel [conversation]="active()" />
    </div>
  `,
})
export class ConversationsShellComponent {
  active = signal<ConversationResponse | null>(null);

  onSelect(conv: ConversationResponse): void {
    this.active.set(conv);
  }

  onClosed(): void {
    if (this.active()) {
      this.active.update(c => c ? { ...c, status: 'Closed' } : null);
    }
  }
}
