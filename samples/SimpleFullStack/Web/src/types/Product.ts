import type { Category } from './Category';

export interface Product {
	productID: number;
	name: string;
	description?: string;
	sku: string;
	categoryID: number;
	brand?: string;
	isActive: boolean;
	category: Category;
}

export interface AddProduct {
	name: string;
	description?: string;
	sku: string;
	categoryID: number;
	brand?: string;
	isActive: boolean;
}

export interface UpdateProduct {
	name?: string;
	description?: string;
	sku?: string;
	categoryID?: number;
	brand?: string;
	isActive?: boolean;
}
