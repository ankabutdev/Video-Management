import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Product, ProductCreate } from "../interfaces/Product";
import { catchError } from "rxjs";

@Injectable({
    providedIn: "root"
})

export class VideoService {

    constructor(private http: HttpClient) { 
        
    }

    url = 'https://localhost:7265/api/products'
    urlFile = 'https://localhost:7265/api/file'

    async getAllProducts() {
        return await this.http.get<Product[]>(this.url)
            .pipe(
                catchError((error: any) => {
                    console.error('Error:', error);
                    throw error;
                })
            );
    }

    async search(query: string) {
        return await this.http.get<Product[]>(this.url + "/search?search=" + query);
    }

    async getById(id: number) {
        return await this.http.get<Product>(this.url + "/" + id)
    }

    async create(data: any) {
        return await this.http.post(this.url, data)
    }

    async update(id: number, data: any) {
        return await this.http.put(this.url + "/" + id, data);
    }

    async delete(id: number) {
        return await this.http.delete(this.url + "/" + id);
    }
}