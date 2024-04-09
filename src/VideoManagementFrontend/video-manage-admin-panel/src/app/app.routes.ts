import { Routes } from '@angular/router';
import { ProductsComponent } from './components/products/products.component';
import { CreateProductComponent } from './create-product/create-product.component';
import { NotfoundComponent } from './notfound/notfound.component';
import { UpdateProductComponent } from './update-product/update-product.component';

export const routes: Routes = [
    { path: 'products', component: ProductsComponent },
    { path: 'createProduct', component: CreateProductComponent },
    { path: '', redirectTo: 'products', pathMatch: 'full' },
    { path: 'update/:id', component: UpdateProductComponent, title: 'Product update' },
    { path: '**', component: NotfoundComponent }
];
