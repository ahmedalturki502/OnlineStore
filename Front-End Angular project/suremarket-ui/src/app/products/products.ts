import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ProductService, Product } from '../services/product.service';
import { CartService } from '../services/cart.service';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './products.html',
  styleUrls: ['./products.css']
})
export class ProductsComponent implements OnInit {
  products: Product[] = [];
  isLoading = true;
  errorMessage = '';

  constructor(
    private productService: ProductService,
    private cartService: CartService
  ) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.isLoading = true;
    this.errorMessage = '';
    
    this.productService.getAllProducts().subscribe({
      next: (products) => {
        this.products = products;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading products:', error);
        this.errorMessage = error;
        this.isLoading = false;
        this.products = [];
      }
    });
  }

  addToCart(product: Product): void {
    if (product.stockQuantity > 0) {
      this.cartService.addToCart(product.id, 1).subscribe({
        next: (cart) => {
          console.log(`Added ${product.name} to cart`);
          this.showAddToCartConfirmation(product.name);
        },
        error: (error) => {
          console.error('Error adding to cart:', error);
          alert(`Failed to add ${product.name} to cart: ${error}`);
        }
      });
    }
  }

  private showAddToCartConfirmation(productName: string): void {
    // Simple confirmation - can be replaced with a proper notification system later
    alert(`${productName} has been added to your cart!`);
  }
}