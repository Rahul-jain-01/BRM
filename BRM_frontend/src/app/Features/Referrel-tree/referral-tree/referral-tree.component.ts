import { Component, inject } from '@angular/core';
import { ReferralService, ReferralTreeDto } from '../../../core/Services/referral.service';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { TokenService } from '../../../core/Services/token.service';

@Component({
  selector: 'app-referral-tree',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './referral-tree.component.html',
  styleUrl: './referral-tree.component.css'
})
export class ReferralTreeComponent {
  private tokenService = inject(TokenService);
  userId!: number;
  referralTree: ReferralTreeDto | null = null;
  isPaid?: boolean= false; // Initialize isPaid to false
  constructor(private route: ActivatedRoute, private referralService: ReferralService) {}
  ngOnInit() {
    this.isPaid = this.tokenService.isUserPaid(); // decode from token
    console.log('Is Paid:', this.isPaid);
    
    this.userId = this.tokenService.getUserIdFromToken() as number; 
    if (this.userId) {
      this.loadReferralTree();
    }
  }
  
  loadReferralTree() {
    this.referralService.getReferralTree(this.userId).subscribe({
      next: (data) => {
        this.referralTree = data;
      },
      error: (err) => {
        console.error('Error fetching referral tree:', err);
      }
    });
  }
}
