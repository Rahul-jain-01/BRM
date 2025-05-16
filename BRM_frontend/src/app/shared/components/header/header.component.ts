import { Component, inject } from '@angular/core';
import { AuthService } from '../../../Features/Auth/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { filter } from 'rxjs';
import { NavigationEnd } from '@angular/router';
@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  
isOnRegisterPage = false;
authService=inject(AuthService);

 constructor(private router: Router) {
   this.router.events
   .pipe(filter(event => event instanceof NavigationEnd))
   .subscribe((event: NavigationEnd) => {
   this.isOnRegisterPage = event.urlAfterRedirects === '/register';
   });
 }

isLoggedIn = this.authService.isLoggedIn;

logout(){
  this.authService.logout();
}
   navigateToAuth() {
   if (this.isOnRegisterPage) {
   this.router.navigate(['/login']);
   } else {
   this.router.navigate(['/register']);
 }
}
}
