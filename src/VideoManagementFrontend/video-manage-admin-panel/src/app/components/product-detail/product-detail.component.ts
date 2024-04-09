import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Product } from '../../../interfaces/Product';
import { VideoService } from '../../../services/videoService';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './product-detail.component.html',
  styleUrl: './product-detail.component.css'
})

export class ProductDetailComponent {
  @Input()
  data!: Product;

  constructor(private _service: VideoService) {

  }

  async deletee(id: number) {
    alert("Do you want to delete this item!!!");

    (await this._service.delete(id)).subscribe(response => {
      console.log('delete successfully');
    },
      error => {
        console.error('Error: ', error);
      })
  }
}
