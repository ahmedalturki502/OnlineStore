import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CartService, CartItem } from '../services/cart.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cart.html',
  styleUrls: ['./cart.css']
})
export class CartComponent implements OnInit, OnDestroy {
  cartItems: CartItem[] = [];
  totalPrice: number = 0;
  itemCount: number = 0;
  private cartSubscription?: Subscription;

  constructor(private cartService: CartService) {}

  ngOnInit(): void {
    this.cartSubscription = this.cartService.getCartItems().subscribe(items => {
      this.cartItems = items;
      this.updateCartSummary();
    });
  }

  ngOnDestroy(): void {
    if (this.cartSubscription) {
      this.cartSubscription.unsubscribe();
    }
  }

  private updateCartSummary(): void {
    this.totalPrice = this.cartService.getTotalPrice();
    this.itemCount = this.cartService.getItemCount();
  }

  increaseQuantity(itemId: string): void {
    this.cartService.increaseQuantity(itemId);
  }

  decreaseQuantity(itemId: string): void {
    this.cartService.decreaseQuantity(itemId);
  }

  updateQuantity(itemId: string, event: Event): void {
    const target = event.target as HTMLInputElement;
    // Convert to English numerals and parse
    const englishValue = this.toEnglishNumerals(target.value);
    const quantity = parseInt(englishValue, 10);
    
    if (!isNaN(quantity) && quantity >= 0) {
      this.cartService.updateQuantity(itemId, quantity);
      // Update the input value to show English numerals
      target.value = this.toEnglishNumerals(quantity);
    }
  }

  removeItem(itemId: string): void {
    this.cartService.removeItem(itemId);
  }

  getSubtotal(item: CartItem): number {
    return this.cartService.getSubtotal(item);
  }

  clearCart(): void {
    if (confirm('Are you sure you want to clear your cart?')) {
      this.cartService.clearCart();
    }
  }

  proceedToCheckout(): void {
    if (this.cartItems.length === 0) {
      alert('Your cart is empty. Please add items before checkout.');
      return;
    }
    
    // Placeholder for checkout functionality
    alert(`Proceeding to checkout with ${this.itemCount} items totaling $${this.totalPrice.toFixed(2)}`);
    // TODO: Implement actual checkout functionality when backend is ready
  }

  isCartEmpty(): boolean {
    return this.cartItems.length === 0;
  }

  trackByItemId(index: number, item: CartItem): string {
    return item.id;
  }

  toEnglishNumerals(value: number | string): string {
    return value.toString().replace(/[٠-٩]/g, d => '٠١٢٣٤٥٦٧٨٩'.indexOf(d).toString());
  }
}
