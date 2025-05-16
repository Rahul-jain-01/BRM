import { Routes } from '@angular/router';
import { ReferralTreeComponent } from './Features/Referrel-tree/referral-tree/referral-tree.component';
import { LoginComponent } from './Features/Auth/login/login.component';
import { AuthGuard } from './core/Guards/auth.guard';
import { RegisterComponent } from './Features/Auth/register/register.component';
import { RoleGuard } from './core/Guards/role.guard';
import { AdminDashboardComponent } from './Admin/components/admin-dashboard/admin-dashboard.component';

export const routes: Routes = [
  {
    path: 'register',
    component: RegisterComponent
  },
  {
    path: 'referrals',
    component: ReferralTreeComponent,
    canActivate:[AuthGuard,RoleGuard],
    data: { roles: ['User'] },
  },
  {
    path: 'admin',
    // loadComponent: () =>
    //   import('./Admin/components/admin-dashboard/admin-dashboard.component').then(m =>m.AdminDashboardComponent),
    component:AdminDashboardComponent,
    canActivate: [AuthGuard,RoleGuard],
    data: { roles: ['Admin'] },
  },
  {
    path: '**',
    component:LoginComponent,
  }


  // {
  //   path: 'ref',
  //   redirectTo: 'referrals/1',
  //   pathMatch: 'full',

  // },


];
