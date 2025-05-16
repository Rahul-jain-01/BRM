import { Component } from '@angular/core';
import { User } from '../../../core/Models/user.model';
import { AdminService } from '../../services/admin.service';
import { CommonModule } from '@angular/common';
import { ReferralTreeDto } from '../../../core/Services/referral.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.css',
})
export class AdminDashboardComponent {
  users: User[] = [];
  
  searchTerm: string = '';

  showTreeModal: boolean = false;
  selectedUser: User | null = null;
  referralTree: ReferralTreeDto | null = null;

  constructor(private adminService: AdminService) {}

  ngOnInit() {
    this.fetchUsers();
    this.filteredUsers();
  }

  fetchUsers() {
    this.adminService.getAllUsers().subscribe((users) => (this.users = users));
  }

  togglePaid(user: User) {
    const updatedStatus = !user.isPaid;
    this.adminService
      .updateIsPaidStatus(user.email, updatedStatus)
      .subscribe(() => {
        user.isPaid = updatedStatus;
      });
  }

  getReferralCode(user: User) {
    this.adminService
      .getReferralCodeByEmail(user.email)
      .subscribe((referral) => {
        alert(`Referral Code: ${referral.referralCode}`);
      });
  }

  
 filteredUsers(): User[] {
    if (!this.searchTerm) return this.users;
    const term = this.searchTerm.toLowerCase();
    return this.users.filter(user =>
  
      user.email.toLowerCase().includes(term)
    );
  }

  onSearchChange(value: string) {
    this.searchTerm = value;
    this.filteredUsers();
  }
  

  openTreeModal(user: User) {
    this.selectedUser = user;
    this.showTreeModal = true;

    this.adminService
      .getUserReferralTreeByEmail(user.email)
      .subscribe((tree) => {
        this.referralTree = tree;
      });
  }

  closeTreeModal() {
    this.showTreeModal = false;
    this.selectedUser = null;
    this.referralTree = null;
  }
}
