export interface ProductAttribute {
	attributeID: number;
	productID: number;
	attributeName: string;
	attributeValue: string;
}

export interface AddProductAttribute {
	productID: number;
	attributeName: string;
	attributeValue: string;
}

export interface UpdateProductAttribute {
	attributeName: string;
	attributeValue: string;
}
