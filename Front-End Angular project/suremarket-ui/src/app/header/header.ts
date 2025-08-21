import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CategoryService, Category } from '../services/category.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './header.html',
  styleUrls: ['./header.css']
})
export class HeaderComponent implements OnInit {
  dropdownOpen = false;
  categories: Category[] = [];

  constructor(private categoryService: CategoryService) {}

  ngOnInit(): void {
    this.categoryService.getCategories().subscribe({
      next: (cats) => (this.categories = cats || []),
      error: () => (this.categories = [])
    });
  }

  toggleDropdown() {
    this.dropdownOpen = !this.dropdownOpen;
  }

  closeDropdown() {
    this.dropdownOpen = false;
  }
}