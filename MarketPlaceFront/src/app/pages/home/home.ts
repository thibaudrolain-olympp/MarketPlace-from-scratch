import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { Product, Productsservice } from '../../services/productsservice';
import { Router } from '@angular/router';
import { AuthService } from '../../services/authentification';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, MatToolbarModule, MatButtonModule, MatCardModule],
  templateUrl: './home.html',
  styleUrls: ['./home.css']
})
export class HomeComponent implements OnInit {
  products!: Product[];

  constructor(private productsService: Productsservice,  private authService: AuthService,
    private router: Router) {}

  ngOnInit() {
    this.productsService.getProducts().subscribe(data => {
      this.products = data;
    });
  }

  getMainImage(product: Product): string {
    const mainImage = product.images?.find(img => img.isMain);
    return mainImage ? mainImage.image : 'assets/default-product-image.jpg';
  }
}
