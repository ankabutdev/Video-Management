import { Component, Input, Renderer2 } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogActions, MatDialogClose, MatDialogContent, MatDialogRef, MatDialogTitle } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormGroup, Validators } from '@angular/forms';
import { Product, ProductCreate } from '../../interfaces/Product';
import { VideoService } from '../../services/videoService';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-update-product',
  standalone: true,
  imports: [MatFormFieldModule,
    ReactiveFormsModule,
    MatInputModule,
    FormsModule,
    MatButtonModule,
    MatDialogTitle,
    MatDialogContent,
    MatDialogActions,
    MatDialogClose,
    RouterLink],
  templateUrl: './update-product.component.html',
  styleUrl: './update-product.component.css'
})

export class UpdateProductComponent {
  product!: Product;
  productC!: ProductCreate;
  applyForm!: FormGroup;
  productId!: number;

  constructor(private _videoService: VideoService, private route: ActivatedRoute,
    private fb: FormBuilder, private formBuilder: FormBuilder, private router: Router
  ) {
    this.productId = parseInt(this.route.snapshot.params['id'], 10);
    this.getProductIdFromUrl(this.productId)
  }

  private async getProductIdFromUrl(productId: number) {
    (await this._videoService.getById(productId)).subscribe(response => {
      this.product = response;

      this.applyForm = this.formBuilder.group({
        Name: [this.product.name, Validators.required],
        Description: [this.product.description, Validators.required],
        Video: [this.product.videoUrl, Validators.required]
      });
    })
  }

  globalval!: File;

  async updateProduct() {

    let res = this.applyForm.getRawValue()
    let data = this.getFormData(res);

    (await this._videoService.update(this.productId, data)).subscribe(response => {
      console.log('Product updated', response);
      this.applyForm.reset();
      this.router.navigate(['/products'])
      
    }, (error: any) => {
      console.error('Error creating product', error);
    });
  }

  onFileSelected(event: any) {
    this.globalval = <File>event.target.files[0];
  }

  getFormData(object: any) {
    let data = new FormData();
    for (let [key, val] of Object.entries(object)) {
      if (key == "Video") {
        data.append('Video', this.globalval)
      }
      else data.append(key, JSON.stringify(val));
    }
    return data;
  }
}
