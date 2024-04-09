import { Component } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogActions, MatDialogClose, MatDialogContent, MatDialogRef, MatDialogTitle } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormGroup, Validators } from '@angular/forms';
import { ProductCreate } from '../../interfaces/Product';
import { VideoService } from '../../services/videoService';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-create-product',
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
  templateUrl: './create-product.component.html',
  styleUrl: './create-product.component.css'
})

export class CreateProductComponent {
  product!: ProductCreate;
  applyForm!: FormGroup;

  constructor(
    private videoService: VideoService,
    private formBuilder: FormBuilder, private router: Router
  ) {

    this.applyForm = this.formBuilder.group({
      Name: ['', Validators.required],
      Description: ['', Validators.required],
      Video: [File, Validators.required]
    });
  }

  globalval!: File;

  async createProduct() {
    
    let res = this.applyForm.getRawValue()
    let data = this.getFormData(res);

    (await this.videoService.create(data)).subscribe(response => {
      console.log('Product created', response);
      this.applyForm.reset();
      this.router.navigate(['/products']);
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
      if (key == 'Video') {
        data.append('Video', this.globalval)
      }
      else data.append(key, JSON.stringify(val));
    }
    return data;
  }
}
