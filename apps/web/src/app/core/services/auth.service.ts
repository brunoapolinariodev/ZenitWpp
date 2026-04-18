import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { AuthResponse, LoginRequest, AgentInfo } from '../models/auth.model';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly TOKEN_KEY = 'zenitwpp-token';
  private readonly AGENT_KEY = 'zenitwpp-agent';

  private _agent = signal<AgentInfo | null>(this.loadAgent());

  agent = this._agent.asReadonly();
  isAuthenticated = computed(() => this._agent() !== null);

  constructor(private http: HttpClient, private router: Router) {}

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${environment.apiUrl}/auth/login`, request).pipe(
      tap(response => {
        if (!response.requiresTwoFactor) {
          this.saveSession(response);
        }
      })
    );
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.AGENT_KEY);
    this._agent.set(null);
    this.router.navigate(['/auth/login']);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  private saveSession(response: AuthResponse): void {
    localStorage.setItem(this.TOKEN_KEY, response.accessToken);
    localStorage.setItem(this.AGENT_KEY, JSON.stringify(response.agent));
    this._agent.set(response.agent);
  }

  private loadAgent(): AgentInfo | null {
    const raw = localStorage.getItem(this.AGENT_KEY);
    return raw ? JSON.parse(raw) : null;
  }
}
