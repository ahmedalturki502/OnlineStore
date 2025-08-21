import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

export interface Category {
  id: string;
  name: string;
}

@Injectable({ providedIn: 'root' })
export class CategoryService {
  private readonly baseUrl = '/api/Categories';

  constructor(private http: HttpClient) {}

  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(this.baseUrl).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let message = 'Failed to load categories';
    if (error.error instanceof ErrorEvent) {
      message = error.error.message;
    } else if (error.status === 500) {
      message = 'Server error while fetching categories';
    }
    console.error('CategoryService Error:', message);
    return throwError(() => message);
  }
}


