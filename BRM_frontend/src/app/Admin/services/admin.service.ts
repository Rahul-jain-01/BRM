import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../../core/Models/user.model';
import { ReferralTreeDto } from '../../core/Services/referral.service';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private apiUrl = 'https://localhost:7189/api/Admin';

  constructor(private http: HttpClient) {}

  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${this.apiUrl}/users`);
  }

  updateIsPaidStatus(userEmail:string, isPaid: boolean): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/mark-paid/${userEmail}`, { isPaid });
  }
  getReferralCodeByEmail(email: string): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/user-referral-code/${email}`);
  }

  getUserReferralTreeByEmail(email: string): Observable<ReferralTreeDto> {
    return this.http.get<ReferralTreeDto>(`${this.apiUrl}/referral-tree/${email}`);
  }
}
