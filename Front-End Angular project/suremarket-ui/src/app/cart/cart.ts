import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CartService, CartItem, Cart } from '../services/cart.service';
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
  isLoading = true;
  errorMessage = '';
  private cartSubscription?: Subscription;

  constructor(private cartService: CartService) {}

  ngOnInit(): void {
    this.loadCart();
    this.subscribeToCartChanges();
  }

  loadCart(): void {
    this.isLoading = true;
    this.errorMessage = '';
    
    this.cartService.getCart().subscribe({
      next: (cart) => {
        this.updateCartData(cart);
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading cart:', error);
        this.errorMessage = error;
        this.isLoading = false;
        this.cartItems = [];
        this.totalPrice = 0;
        this.itemCount = 0;
      }
    });
  }

  private subscribeToCartChanges(): void {
    this.cartSubscription = this.cartService.cart$.subscribe(cart => {
      this.updateCartData(cart);
    });
  }

  private updateCartData(cart: Cart): void {
    this.cartItems = cart.items;
    this.totalPrice = cart.total;
    this.itemCount = cart.items.reduce((total, item) => total + item.quantity, 0);
  }

  ngOnDestroy(): void {
    if (this.cartSubscription) {
      this.cartSubscription.unsubscribe();
    }
  }

  increaseQuantity(productId: string): void {
    this.cartService.increaseQuantity(productId).subscribe({
      next: (cart) => this.updateCartData(cart),
      error: (error) => {
        console.error('Error increasing quantity:', error);
        this.errorMessage = error;
      }
    });
  }

  decreaseQuantity(productId: string): void {
    this.cartService.decreaseQuantity(productId).subscribe({
      next: (cart) => this.updateCartData(cart),
      error: (error) => {
        console.error('Error decreasing quantity:', error);
        this.errorMessage = error;
      }
    });
  }

  updateQuantity(productId: string, event: Event): void {
    const target = event.target as HTMLInputElement;
    // Convert to English numerals and parse
    const englishValue = this.toEnglishNumerals(target.value);
    const quantity = parseInt(englishValue, 10);
    
    if (!isNaN(quantity) && quantity >= 0) {
      this.cartService.updateQuantity(productId, quantity).subscribe({
        next: (cart) => {
          this.updateCartData(cart);
          // Update the input value to show English numerals
          target.value = this.toEnglishNumerals(quantity);
        },
        error: (error) => {
          console.error('Error updating quantity:', error);
          this.errorMessage = error;
          // Reset input to previous value
          const item = this.cartItems.find(item => item.productId === productId);
          if (item) {
            target.value = this.toEnglishNumerals(item.quantity);
          }
        }
      });
    }
  }

  removeItem(productId: string): void {
    this.cartService.removeItem(productId).subscribe({
      next: (cart) => this.updateCartData(cart),
      error: (error) => {
        console.error('Error removing item:', error);
        this.errorMessage = error;
      }
    });
  }

  getSubtotal(item: CartItem): number {
    return item.total;
  }

  clearCart(): void {
    if (confirm('Are you sure you want to clear your cart?')) {
      this.cartService.clearCart().subscribe({
        next: () => {
          this.updateCartData({ items: [], total: 0 });
        },
        error: (error) => {
          console.error('Error clearing cart:', error);
          this.errorMessage = error;
        }
      });
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
    return item.productId;
  }

  toEnglishNumerals(value: number | string): string {
    return value.toString().replace(/[٠-٩]/g, d => '٠١٢٣٤٥٦٧٨٩'.indexOf(d).toString());
  }
}
