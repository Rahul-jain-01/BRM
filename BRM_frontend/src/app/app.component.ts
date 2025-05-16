import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from "./shared/components/header/header.component";
import { Router, NavigationEnd } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TokenService } from './core/Services/token.service';
// import { UserListComponent } from "./Admin/components/admin-dashboard/admin-dashboard.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'ChainMarketing';
  authService=inject(TokenService); 
  
// showHeader = true;

//   constructor(private router: Router) {
//     this.router.events.subscribe(event => {
//       if (event instanceof NavigationEnd) {
//         this.showHeader = !event.url.includes('/register');
//       }
//     });
//   }

}
