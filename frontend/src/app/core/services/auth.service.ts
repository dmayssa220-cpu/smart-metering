import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { Router } from '@angular/router';

export interface AuthResponse {
  token: string;
  email: string;
  fullName: string;
  role: string;
}

const TOKEN_KEY = 'sm_token';
const USER_KEY = 'sm_user';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly base = '/api/auth';

  // Signal exposant l'utilisateur courant (null si déconnecté)
  currentUser = signal<Omit<AuthResponse, 'token'> | null>(this.readStoredUser());

  constructor(private http: HttpClient, private router: Router) {}

  login(email: string, password: string): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.base}/login`, { email, password }).pipe(
      tap((res) => this.storeSession(res))
    );
  }

  logout(): void {
    localStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(USER_KEY);
    this.currentUser.set(null);
    this.router.navigate(['/login']);
  }

  get token(): string | null {
    return localStorage.getItem(TOKEN_KEY);
  }

  isAuthenticated(): boolean {
    return !!this.token;
  }

  private storeSession(res: AuthResponse): void {
    localStorage.setItem(TOKEN_KEY, res.token);
    const user = { email: res.email, fullName: res.fullName, role: res.role };
    localStorage.setItem(USER_KEY, JSON.stringify(user));
    this.currentUser.set(user);
  }

  private readStoredUser(): Omit<AuthResponse, 'token'> | null {
    const raw = localStorage.getItem(USER_KEY);
    return raw ? JSON.parse(raw) : null;
  }
}
