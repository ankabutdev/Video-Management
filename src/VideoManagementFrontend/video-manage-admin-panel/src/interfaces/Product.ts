export interface Product {
    id: number,
    name: string,
    videoUrl: string,
    description: string,
    sortNumber:number
}

export interface ProductCreate {
    Name: string,
    Description: string,
    Video: File | undefined,
}