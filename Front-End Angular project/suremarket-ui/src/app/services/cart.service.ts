import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

export interface CartItem {
  id: string;
  name: string;
  price: number;
  quantity: number;
  imageUrl: string;
}

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartItemsSubject = new BehaviorSubject<CartItem[]>([
    {
      id: 'headphones1',
      name: 'Premium Wireless Headphones',
      price: 149.99,
      quantity: 2,
      imageUrl: 'https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=300&h=300&fit=crop'
    },
    {
      id: 'smartwatch1',
      name: 'Smart Fitness Watch',
      price: 299.99,
      quantity: 1,
      imageUrl: 'https://images.unsplash.com/photo-1523275335684-37898b6baf30?w=300&h=300&fit=crop'
    },
    {
      id: 'speaker1',
      name: 'Portable Bluetooth Speaker',
      price: 89.99,
      quantity: 3,
      imageUrl: 'https://images.unsplash.com/photo-1608043152269-423dbba4e7e1?w=300&h=300&fit=crop'
    }
  ]);

  public cartItems$ = this.cartItemsSubject.asObservable();

  constructor() { }

  getCartItems(): Observable<CartItem[]> {
    return this.cartItems$;
  }

  getCurrentCartItems(): CartItem[] {
    return this.cartItemsSubject.value;
  }

  addItem(item: CartItem): void {
    const currentItems = this.getCurrentCartItems();
    const existingItemIndex = currentItems.findIndex(cartItem => cartItem.id === item.id);

    if (existingItemIndex > -1) {
      // Item exists, update quantity
      currentItems[existingItemIndex].quantity += item.quantity;
    } else {
      // New item, add to cart
      currentItems.push(item);
    }

    this.cartItemsSubject.next([...currentItems]);
  }

  updateQuantity(itemId: string, quantity: number): void {
    if (quantity < 1) {
      this.removeItem(itemId);
      return;
    }

    const currentItems = this.getCurrentCartItems();
    const itemIndex = currentItems.findIndex(item => item.id === itemId);

    if (itemIndex > -1) {
      currentItems[itemIndex].quantity = quantity;
      this.cartItemsSubject.next([...currentItems]);
    }
  }

  increaseQuantity(itemId: string): void {
    const currentItems = this.getCurrentCartItems();
    const item = currentItems.find(item => item.id === itemId);
    
    if (item) {
      this.updateQuantity(itemId, item.quantity + 1);
    }
  }

  decreaseQuantity(itemId: string): void {
    const currentItems = this.getCurrentCartItems();
    const item = currentItems.find(item => item.id === itemId);
    
    if (item && item.quantity > 1) {
      this.updateQuantity(itemId, item.quantity - 1);
    }
  }

  removeItem(itemId: string): void {
    const currentItems = this.getCurrentCartItems();
    const filteredItems = currentItems.filter(item => item.id !== itemId);
    this.cartItemsSubject.next(filteredItems);
  }

  clearCart(): void {
    this.cartItemsSubject.next([]);
  }

  getItemCount(): number {
    return this.getCurrentCartItems().reduce((total, item) => total + item.quantity, 0);
  }

  getTotalPrice(): number {
    return this.getCurrentCartItems().reduce((total, item) => total + (item.price * item.quantity), 0);
  }

  getSubtotal(item: CartItem): number {
    return item.price * item.quantity;
  }
}
