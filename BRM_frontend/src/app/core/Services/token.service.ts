import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
@Injectable({
  providedIn: 'root'
})
export class TokenService {

  private jwtHelper = new JwtHelperService();

  getDecodedToken(): any {
    const token = localStorage.getItem('token');
    return token ? this.jwtHelper.decodeToken(token) : null;
  }

  getUserId(): number | null {
    const decoded = this.getDecodedToken();
    return decoded?.nameid ? +decoded.nameid : null;
  }

  getUserRole(): string | null {
    const decoded = this.getDecodedToken();
    return decoded?.['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ?? null;
  }
  
  isUserPaid(): boolean {
    const token = this.getDecodedToken();
    const isPaidStr = token?.isPaid;
  
    // Convert "True"/"False" string to boolean
    return isPaidStr?.toLowerCase() === 'true';
  }
  

  isTokenExpired(): boolean {
    const token = localStorage.getItem('token');
    return token ? this.jwtHelper.isTokenExpired(token) : true;
  }
  getUserIdFromToken(): number | null {
    const token = localStorage.getItem('token');
    if (!token) return null;
  
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload.sub ? +payload.sub : null; // or payload['userId'] if you used a custom claim
  }
}
