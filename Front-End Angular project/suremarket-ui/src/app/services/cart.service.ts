import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

export interface CartItem {
  productId: string;
  productName: string;
  productImageUrl: string;
  price: number;
  quantity: number;
  total: number;
}

export interface Cart {
  items: CartItem[];
  total: number;
}

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private readonly baseUrl = '/api/Cart';
  private cartSubject = new BehaviorSubject<Cart>({ items: [], total: 0 });
  public cart$ = this.cartSubject.asObservable();

  constructor(private http: HttpClient) {}

  getCart(): Observable<Cart> {
    return this.http.get<Cart>(this.baseUrl)
      .pipe(
        tap(cart => this.cartSubject.next(cart)),
        catchError(this.handleError)
      );
  }

  addToCart(productId: string, quantity: number): Observable<Cart> {
    return this.http.post<Cart>(`${this.baseUrl}/items`, { productId, quantity })
      .pipe(
        tap(cart => this.cartSubject.next(cart)),
        catchError(this.handleError)
      );
  }

  updateQuantity(productId: string, quantity: number): Observable<Cart> {
    if (quantity < 1) {
      return this.removeItem(productId);
    }

    return this.http.put<Cart>(`${this.baseUrl}/items/${productId}`, { quantity })
      .pipe(
        tap(cart => this.cartSubject.next(cart)),
        catchError(this.handleError)
      );
  }

  increaseQuantity(productId: string): Observable<Cart> {
    const currentCart = this.cartSubject.value;
    const item = currentCart.items.find(item => item.productId === productId);
    
    if (item) {
      return this.updateQuantity(productId, item.quantity + 1);
    }
    
    return throwError(() => 'Item not found in cart');
  }

  decreaseQuantity(productId: string): Observable<Cart> {
    const currentCart = this.cartSubject.value;
    const item = currentCart.items.find(item => item.productId === productId);
    
    if (item && item.quantity > 1) {
      return this.updateQuantity(productId, item.quantity - 1);
    }
    
    return throwError(() => 'Cannot decrease quantity below 1');
  }

  removeItem(productId: string): Observable<Cart> {
    return this.http.delete<Cart>(`${this.baseUrl}/items/${productId}`)
      .pipe(
        tap(cart => this.cartSubject.next(cart)),
        catchError(this.handleError)
      );
  }

  clearCart(): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/clear`)
      .pipe(
        tap(() => this.cartSubject.next({ items: [], total: 0 })),
        catchError(this.handleError)
      );
  }

  getCurrentCart(): Cart {
    return this.cartSubject.value;
  }

  getItemCount(): number {
    return this.getCurrentCart().items.reduce((total, item) => total + item.quantity, 0);
  }

  getTotalPrice(): number {
    return this.getCurrentCart().total;
  }

  getSubtotal(item: CartItem): number {
    return item.total;
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'An unexpected error occurred';
    
    if (error.error instanceof ErrorEvent) {
      // Client-side error
      errorMessage = error.error.message;
    } else {
      // Server-side error
      if (error.status === 401) {
        errorMessage = 'Please log in to access your cart';
      } else if (error.status === 404) {
        errorMessage = 'Cart or item not found';
      } else if (error.status === 400) {
        errorMessage = error.error?.message || 'Invalid request';
      } else if (error.status === 500) {
        errorMessage = 'Server error. Please try again later';
      } else {
        errorMessage = `Error: ${error.status} - ${error.statusText}`;
      }
    }
    
    console.error('CartService Error:', errorMessage);
    return throwError(() => errorMessage);
  }
}
