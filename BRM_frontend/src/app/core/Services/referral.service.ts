import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface ReferralUserDto {
  id: number;
  hasCoApplicant: boolean;
}

export interface ReferralTreeDto {
  userId: number;
  level1: ReferralUserDto[];
  level2: ReferralUserDto[];
  level3: ReferralUserDto[];
}
@Injectable({
  providedIn: 'root'
})
export class ReferralService {

  private apiUrl = 'https://localhost:7189/api/ReferralPath';

  constructor(private http: HttpClient) {}

  getReferralTree(userId: number): Observable<ReferralTreeDto> {
    return this.http.get<ReferralTreeDto>(`${this.apiUrl}/${userId}`);
  }
}
