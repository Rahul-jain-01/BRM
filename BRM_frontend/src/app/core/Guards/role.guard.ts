import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../../Features/Auth/auth.service';
import { TokenService } from '../Services/token.service';

export const RoleGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const tokenService = inject(TokenService);
  const router = inject(Router);
  const expectedRoles = route.data['roles'];

  const userRole = tokenService.getUserRole(); // Decode from JWT
  
  if (!expectedRoles.includes(userRole)) {
    router.navigate(['/unauthorized']);
    return false;
  }

  return true;
};
