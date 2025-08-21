import { Injectable } from '@angular/core';
import { Observable, forkJoin, of } from 'rxjs';
import { map, catchError, switchMap } from 'rxjs/operators';
import { CategoryService, Category } from './category.service';
import { ProductService, Product } from './product.service';

export interface FeaturedSection {
  categoryId: string;
  categoryName: string;
  products: Product[];
}

@Injectable({ providedIn: 'root' })
export class HomeService {
  constructor(
    private categoryService: CategoryService,
    private productService: ProductService
  ) {}

  getFeaturedProducts(): Observable<FeaturedSection[]> {
    return this.categoryService.getCategories().pipe(
      catchError(() => of([] as Category[])),
      map((categories: Category[]) => categories || []),
      // For each category, fetch products and take first 3
      // Use forkJoin to combine into an array
      switchMap((categories: Category[]) => {
        if (!categories.length) {
          return of([] as FeaturedSection[]);
        }

        const perCategory$: Observable<FeaturedSection>[] = categories.map((cat: Category) =>
          this.productService.getProductsByCategory(cat.id).pipe(
            map((products) => ({
              categoryId: cat.id,
              categoryName: cat.name,
              products: (products || []).slice(0, 3)
            }))
          )
        );

        return forkJoin(perCategory$).pipe(
          catchError(() => of([] as FeaturedSection[]))
        );
      })
    );
  }
}


