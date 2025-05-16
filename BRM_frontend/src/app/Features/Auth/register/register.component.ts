import { Component } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule,CommonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  errorMessage = '';
  registerForm: any;
  constructor(private fb: FormBuilder,private authService: AuthService,private router: Router) {}
  ngOnInit() {
    this.initializeForm();
  }
  initializeForm() {
  this.registerForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(3)]],
    referralCode: [''], // Optional
  });
}

  onSubmit() {
    if (this.registerForm.valid) {
      this.authService.register(this.registerForm.value).subscribe({
        next: () => this.router.navigate(['/login']),
        error: (err) =>
          (this.errorMessage = err.error?.message || 'Registration failed'),
      });
    }
  }
}
