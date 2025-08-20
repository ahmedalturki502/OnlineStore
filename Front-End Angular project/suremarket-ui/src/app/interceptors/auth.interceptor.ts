import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const AuthInterceptor: HttpInterceptorFn = (request, next) => {
  const authService = inject(AuthService);
  
  // Get the auth token from the service
  const token = authService.getToken();
  
  // If we have a token, clone the request and add the Authorization header
  if (token) {
    const authRequest = request.clone({
      headers: request.headers.set('Authorization', `Bearer ${token}`)
    });
    return next(authRequest);
  }
  
  // If no token, proceed with the original request
  return next(request);
};
