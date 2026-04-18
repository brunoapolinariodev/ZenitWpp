export interface LoginRequest {
  email: string;
  password: string;
  totpCode?: string;
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  expiresIn: number;
  requiresTwoFactor: boolean;
  agent: AgentInfo;
}

export interface AgentInfo {
  id: string;
  name: string;
  email: string;
  role: 'Operator' | 'Supervisor' | 'Admin';
}
