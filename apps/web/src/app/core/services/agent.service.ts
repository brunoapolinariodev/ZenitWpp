import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface AgentResponse {
  id: string;
  name: string;
  email: string;
  role: 'Operator' | 'Supervisor' | 'Admin';
  isOnline: boolean;
  twoFactorEnabled: boolean;
  createdAt: string;
}

export interface CreateAgentRequest {
  name: string;
  email: string;
  password: string;
  role: string;
}

@Injectable({ providedIn: 'root' })
export class AgentService {
  private http = inject(HttpClient);
  private base = `${environment.apiUrl}/agents`;

  list(): Observable<AgentResponse[]> {
    return this.http.get<AgentResponse[]>(this.base);
  }

  create(req: CreateAgentRequest): Observable<AgentResponse> {
    return this.http.post<AgentResponse>(this.base, req);
  }
}
