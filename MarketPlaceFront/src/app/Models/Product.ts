import { Category } from "./Category";
import { ProductImage } from "./ProductImage";

export interface Product {
  id: number;
  name: string;
  description: string;
  price: number
  quantity: number;
  category: Category;
  images: ProductImage[];
  status: string;
  createdAt: Date;
  updatedAt: Date;
}