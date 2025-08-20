import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ProductService, Product } from '../services/product.service';

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

  constructor(private productService: ProductService) {}

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
      // Placeholder for add to cart functionality
      console.log(`Added ${product.name} to cart`);
      // TODO: Implement actual cart functionality when backend is ready
      this.showAddToCartConfirmation(product.name);
    }
  }

  private showAddToCartConfirmation(productName: string): void {
    // Simple confirmation - can be replaced with a proper notification system later
    alert(`${productName} has been added to your cart!`);
  }
}