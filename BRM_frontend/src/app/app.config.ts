import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { authInterceptor } from '../app/Features/Auth/auth.interceptor'
import { routes } from './app.routes';
import { withInterceptors , provideHttpClient } from '@angular/common/http';

export const appConfig: ApplicationConfig = {
  providers: [provideZoneChangeDetection({ eventCoalescing: true }), provideRouter(routes),provideHttpClient(),
    provideHttpClient(withInterceptors([
      authInterceptor
    ]))
  ],
};
