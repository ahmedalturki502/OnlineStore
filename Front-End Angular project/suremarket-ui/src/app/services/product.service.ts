import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

export interface Product {
  id: string;
  name: string;
  description: string;
  price: number;
  imageUrl: string;
  stockQuantity: number;
  categoryId: string;
  categoryName: string;
}

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private readonly baseUrl = '/api/Products';

  constructor(private http: HttpClient) {}

  // Alias for fetching all products
  getProducts(): Observable<Product[]> {
    return this.getAllProducts();
  }

  getAllProducts(): Observable<Product[]> {
    return this.http.get<{ items: Product[] }>(this.baseUrl)
      .pipe(
        map(response => response.items),
        catchError(this.handleError)
      );
  }

  getProductsByCategory(categoryId: string): Observable<Product[]> {
    const url = `${this.baseUrl}?categoryId=${encodeURIComponent(categoryId)}`;
    return this.http.get<{ items: Product[] }>(url)
      .pipe(
        map(response => response.items),
        catchError(this.handleError)
      );
  }

  getProductById(id: string): Observable<Product> {
    return this.http.get<Product>(`${this.baseUrl}/${id}`)
      .pipe(
        catchError(this.handleError)
      );
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An unexpected error occurred';
    
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = error.error.message;
    } else {
      // Server-side error
      if (error.status === 404) {
        errorMessage = 'Product not found';
      } else if (error.status === 500) {
        errorMessage = 'Server error. Please try again later';
      } else {
        errorMessage = `Error: ${error.status} - ${error.statusText}`;
      }
    }
    
    console.error('ProductService Error:', errorMessage);
    return throwError(() => errorMessage);
  }
}