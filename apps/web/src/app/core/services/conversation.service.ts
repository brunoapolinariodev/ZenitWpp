import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  ConversationResponse,
  ListConversationsResponse,
  ConversationStatus,
} from '../models/conversation.model';

@Injectable({ providedIn: 'root' })
export class ConversationService {
  private http = inject(HttpClient);
  private base = `${environment.apiUrl}/conversations`;

  list(params?: { status?: ConversationStatus; page?: number; pageSize?: number }): Observable<ListConversationsResponse> {
    let p = new HttpParams();
    if (params?.status)   p = p.set('status', params.status);
    if (params?.page)     p = p.set('page', params.page);
    if (params?.pageSize) p = p.set('pageSize', params.pageSize);
    return this.http.get<ListConversationsResponse>(this.base, { params: p });
  }

  get(id: string): Observable<ConversationResponse> {
    return this.http.get<ConversationResponse>(`${this.base}/${id}`);
  }

  sendMessage(id: string, content: string, type = 'Text'): Observable<void> {
    return this.http.post<void>(`${this.base}/${id}/messages`, { content, type });
  }

  transfer(id: string, agentId: string): Observable<void> {
    return this.http.post<void>(`${this.base}/${id}/transfer`, { agentId });
  }

  close(id: string): Observable<void> {
    return this.http.post<void>(`${this.base}/${id}/close`, {});
  }
}
