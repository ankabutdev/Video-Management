import { Component, HostListener } from '@angular/core';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { VideoService } from '../../../services/videoService';
import { Product } from '../../../interfaces/Product';
import { ProductDetailComponent } from '../product-detail/product-detail.component';
import { debounceTime, distinctUntilChanged } from 'rxjs';

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, ProductDetailComponent, ProductsComponent, ReactiveFormsModule],
  templateUrl: './products.component.html',
  styleUrl: './products.component.css'
})

export class ProductsComponent {
  searchInput = new FormControl('');
  productList: Product[] = [];
  productSortList: Product[] = []

  constructor(private _service: VideoService) {
    this.getAllProducts();
    this.searchInput.valueChanges.pipe(
      debounceTime(300), // Wait for 300ms after user stops typing
      distinctUntilChanged() // Only emit when the current value is different from the previous value
    ).subscribe(value => {
      this.search(value);
    });
  }

  async search(query: string | null) {
    if (!query) {
      this.productSortList = this.productList;
      return;
    }

    (await this._service.search(query)).subscribe(response => {
      this.productSortList = response;
      console.log('get successfully');
    },
      error => {
        console.error('Error: ', error);
      });
  }

  @HostListener('document:keydown.enter', ['$event'])
  onEnter(event: KeyboardEvent) {
    this.search(this.searchInput.value);
  }

  async getAllProducts() {
    (await this._service.getAllProducts()).subscribe(
      response => {
        this.productList = response;
        this.productSortList = response;
        console.log('get successfully');
      },
      error => {
        console.error('Error: ', error);
      }
    )
  }
}