import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ContactResponse } from '../models/contact.model';

export interface CreateContactRequest {
  name: string;
  phone: string;
  email?: string;
  segment?: string;
  language?: string;
}

@Injectable({ providedIn: 'root' })
export class ContactService {
  private http = inject(HttpClient);
  private base = `${environment.apiUrl}/contacts`;

  list(): Observable<ContactResponse[]> {
    return this.http.get<ContactResponse[]>(this.base);
  }

  get(id: string): Observable<ContactResponse> {
    return this.http.get<ContactResponse>(`${this.base}/${id}`);
  }

  create(req: CreateContactRequest): Observable<ContactResponse> {
    return this.http.post<ContactResponse>(this.base, req);
  }

  update(id: string, req: Partial<CreateContactRequest>): Observable<void> {
    return this.http.put<void>(`${this.base}/${id}`, req);
  }
}
