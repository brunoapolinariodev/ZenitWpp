import { Injectable, inject } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { environment } from '../../../environments/environment';
import { MessageResponse } from '../models/conversation.model';
import { AuthService } from './auth.service';

export interface NewMessageEvent {
  conversationId: string;
  message: MessageResponse;
}

export interface TypingEvent {
  conversationId: string;
  agentName: string;
}

@Injectable({ providedIn: 'root' })
export class SignalRService {
  private auth = inject(AuthService);
  private hub?: signalR.HubConnection;

  newMessage$ = new Subject<NewMessageEvent>();
  typing$     = new Subject<TypingEvent>();
  connected   = false;

  async connect(): Promise<void> {
    if (this.hub?.state === signalR.HubConnectionState.Connected) return;

    this.hub = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.hubUrl}/chat`, {
        accessTokenFactory: () => this.auth.getToken() ?? '',
      })
      .withAutomaticReconnect()
      .build();

    this.hub.on('ReceiveMessage', (conversationId: string, message: MessageResponse) => {
      this.newMessage$.next({ conversationId, message });
    });

    this.hub.on('AgentTyping', (conversationId: string, agentName: string) => {
      this.typing$.next({ conversationId, agentName });
    });

    await this.hub.start();
    this.connected = true;
  }

  async joinConversation(conversationId: string): Promise<void> {
    await this.hub?.invoke('JoinConversation', conversationId);
  }

  async leaveConversation(conversationId: string): Promise<void> {
    await this.hub?.invoke('LeaveConversation', conversationId);
  }

  async notifyTyping(conversationId: string): Promise<void> {
    await this.hub?.invoke('NotifyTyping', conversationId);
  }

  async disconnect(): Promise<void> {
    await this.hub?.stop();
    this.connected = false;
  }
}
