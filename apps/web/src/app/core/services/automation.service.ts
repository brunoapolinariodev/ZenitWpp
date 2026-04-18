import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export type TriggerType = 'Keyword' | 'FirstMessage' | 'OutsideHours';

export interface FlowResponse {
  id: string;
  name: string;
  triggerType: TriggerType;
  triggerValue?: string;
  isActive: boolean;
  stepsCount: number;
  createdAt: string;
}

export interface CreateFlowRequest {
  name: string;
  triggerType: TriggerType;
  triggerValue?: string;
}

@Injectable({ providedIn: 'root' })
export class AutomationService {
  private http = inject(HttpClient);
  private base = `${environment.apiUrl}/automation`;

  list(): Observable<FlowResponse[]> {
    return this.http.get<FlowResponse[]>(this.base);
  }

  create(req: CreateFlowRequest): Observable<FlowResponse> {
    return this.http.post<FlowResponse>(this.base, req);
  }
}
