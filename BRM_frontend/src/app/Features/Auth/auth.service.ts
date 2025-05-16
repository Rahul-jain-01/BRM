import { HttpClient } from '@angular/common/http';
import { Injectable, signal, computed, effect } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'https://localhost:7189/api/Auth'; 

  // ✅ Reactive auth state
  private tokenSignal = signal<string | null>(localStorage.getItem('token'));

  // ✅ Computed signal for login status
  readonly isLoggedIn = computed(() => !!this.tokenSignal());

  constructor(private http: HttpClient, private router: Router) {}

  login(credentials: { email: string; password: string }) {
    return this.http.post<{ token: string }>(`${this.apiUrl}/login`, credentials).pipe(
      tap((res: { token: string }) => {
        this.tokenSignal.set(res.token);
        localStorage.setItem('token', res.token);
      })
    );
  }
  register(data: {email: string;password: string;referralCode?: string;}) {
    return this.http.post(`${this.apiUrl}/register`, data);
  }
  
  

  logout() {
    localStorage.removeItem('token');
    this.tokenSignal.set(null);
    this.router.navigate(['/login']);
  }
  getToken(): string | null {
    return this.tokenSignal();
  }

  // Optional: for components that still use methods
  isAuthenticated(): boolean {
    return this.isLoggedIn();
  }

  // Optional effect to observe changes (e.g. debugging)
  readonly logAuthState = effect(() => {
    console.log('User logged in:', this.isLoggedIn());
  });
}
