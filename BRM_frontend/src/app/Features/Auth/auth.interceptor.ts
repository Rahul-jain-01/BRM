import { HttpHandler, HttpInterceptor, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { AuthService } from './auth.service';
import { inject } from '@angular/core';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
    const auth = inject(AuthService);
    const token = auth.getToken();
  
    if (token) {
     const authReq = req.clone({
     headers: req.headers.set('Authorization', `Bearer ${token}`)
    });
     return next(authReq);
  }
  
   return next(req);
  };
  
