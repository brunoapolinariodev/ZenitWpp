export interface ContactResponse {
  id: string;
  name: string;
  phone: string;
  email?: string;
  segment?: string;
  language?: string;
  createdAt: string;
}
