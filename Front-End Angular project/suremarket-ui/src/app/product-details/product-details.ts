import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { ProductService, Product } from '../services/product.service';
import { CartService } from '../services/cart.service';

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './product-details.html',
  styleUrls: ['./product-details.css']
})
export class ProductDetailsComponent implements OnInit {
  product: Product | null = null;
  productNotFound = false;
  
  breadcrumbs = [
    { label: 'Home', url: '/' },
    { label: 'Products', url: '/products' },
    { label: '', url: null }
  ];

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService,
    private cartService: CartService
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const productId = params.get('id');
      if (productId) {
        this.loadProduct(productId);
      }
    });
  }

  private loadProduct(productId: string): void {
    this.productService.getProductById(productId).subscribe({
      next: (product) => {
        this.product = product;
        this.productNotFound = false;
        this.breadcrumbs[2].label = product.name;
      },
      error: (error) => {
        console.error('Error loading product:', error);
        this.product = null;
        this.productNotFound = true;
        this.breadcrumbs[2].label = 'Product Not Found';
      }
    });
  }

  addToCart(product: Product): void {
    if (product.stockQuantity > 0) {
      this.cartService.addToCart(product.id, 1).subscribe({
        next: (cart) => {
          console.log(`Added ${product.name} to cart`);
          alert(`${product.name} has been successfully added to your cart!`);
        },
        error: (error) => {
          console.error('Error adding to cart:', error);
          alert(`Failed to add ${product.name} to cart: ${error}`);
        }
      });
    }
  }

  isInStock(): boolean {
    return this.product ? this.product.stockQuantity > 0 : false;
  }

  getStockStatus(): string {
    return this.product && this.product.stockQuantity > 0 ? 'In Stock' : 'Out of Stock';
  }

  getStockStatusClass(): string {
    return this.product && this.product.stockQuantity > 0 ? 'in-stock' : 'out-of-stock';
  }
}
