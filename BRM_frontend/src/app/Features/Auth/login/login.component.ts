import { Component } from '@angular/core';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { TokenService } from '../../../core/Services/token.service';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  email = '';
  password = '';
  
  constructor(private auth: AuthService, private router: Router,private tokenService: TokenService) {}
  
  
  onSubmit() {
    this.auth.login({ email: this.email, password: this.password }).subscribe({
      next: (res) => {
        if(this.tokenService.getUserRole() === 'Admin') {
          console.log(this.tokenService.getUserRole());
          this.router.navigate(['/admin']);
        }else{
          console.log(this.tokenService.getUserRole());
          this.router.navigate(['/referrals']);
        }
        
      },
      error: () => alert('Login failed')
    });
  }
}
