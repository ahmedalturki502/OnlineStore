import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HomeService, FeaturedSection } from '../services/home.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './home.html',
  styleUrls: ['./home.css']
})
export class HomeComponent implements OnInit {
  featured: FeaturedSection[] = [];

  constructor(private homeService: HomeService) {}

  ngOnInit(): void {
    this.homeService.getFeaturedProducts().subscribe({
      next: (sections) => {
        this.featured = sections || [];
      },
      error: () => {
        this.featured = [];
      }
    });
  }
}