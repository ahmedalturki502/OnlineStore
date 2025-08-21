import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { catchError, tap, switchMap, map } from 'rxjs/operators';
import { ProductService } from './product.service';

// Raw interface that might come from the API
interface RawCartItem {
  productId: string;
  productName: string;
  imageUrl?: string;
  productImageUrl?: string;
  product?: {
    imageUrl?: string;
  };
  price: number;
  quantity: number;
  total: number;
}

export interface CartItem {
  productId: string;
  productName: string;
  imageUrl: string;
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

  constructor(private http: HttpClient, private productService: ProductService) {}

  getCart(): Observable<Cart> {
    return this.http.get<Cart>(this.baseUrl)
      .pipe(
        map(cart => this.normalizeCart(cart)),
        switchMap(cart => this.enrichCart(cart)),
        tap(cart => this.cartSubject.next(cart)),
        catchError(this.handleError)
      );
  }

  addToCart(productId: string, quantity: number): Observable<Cart> {
    return this.http.post<Cart>(`${this.baseUrl}/items`, { productId, quantity })
      .pipe(
        map(cart => this.normalizeCart(cart)),
        switchMap(cart => this.enrichCart(cart)),
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
        map(cart => this.normalizeCart(cart)),
        switchMap(cart => this.enrichCart(cart)),
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
        map(cart => this.normalizeCart(cart)),
        switchMap(cart => this.enrichCart(cart)),
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

  private normalizeCart(cart: any): Cart {
    return {
      ...cart,
      items: cart.items.map((item: RawCartItem) => ({
        productId: item.productId,
        productName: item.productName || (item as any).name || '',
        imageUrl: item.imageUrl || item.productImageUrl || item.product?.imageUrl || '',
        price: item.price,
        quantity: item.quantity,
        total: item.total
      }))
    };
  }

  private enrichCart(cart: Cart): Observable<Cart> {
    return this.productService.getAllProducts()
      .pipe(
        map(products => {
          const enrichedItems = cart.items.map(item => {
            const product = products.find(p => p.id === item.productId);
            const imageUrl = product ? product.imageUrl : 'assets/no-image.png';
            const name = product ? product.name : item.productName;
            return {
              ...item,
              imageUrl,
              productName: name,
              // Also include a generic name field for compatibility if used elsewhere
              ...(name ? { name } : {})
            } as CartItem & { name?: string };
          });
          return { ...cart, items: enrichedItems } as Cart;
        })
      );
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
