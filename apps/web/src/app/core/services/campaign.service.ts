import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export type CampaignStatus = 'Draft' | 'Scheduled' | 'Running' | 'Completed' | 'Cancelled';

export interface CampaignResponse {
  id: string;
  name: string;
  message: string;
  status: CampaignStatus;
  segment?: string;
  scheduledAt?: string;
  totalSent: number;
  totalFailed: number;
  createdAt: string;
}

export interface CreateCampaignRequest {
  name: string;
  message: string;
  segment?: string;
  scheduledAt?: string;
}

@Injectable({ providedIn: 'root' })
export class CampaignService {
  private http = inject(HttpClient);
  private base = `${environment.apiUrl}/campaigns`;

  get(id: string): Observable<CampaignResponse> {
    return this.http.get<CampaignResponse>(`${this.base}/${id}`);
  }

  create(req: CreateCampaignRequest): Observable<CampaignResponse> {
    return this.http.post<CampaignResponse>(this.base, req);
  }

  send(id: string): Observable<void> {
    return this.http.post<void>(`${this.base}/${id}/send`, {});
  }
}
