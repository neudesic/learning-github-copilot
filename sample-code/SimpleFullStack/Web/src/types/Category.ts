export interface Category {
	categoryID: number;
	name: string;
	description?: string;
	parentCategoryID?: number;
	subCategories?: Category[];
}

export interface AddCategory {
	name: string;
	description?: string;
	parentCategoryID?: number;
}

export interface UpdateCategoryDescription {
	description?: string;
}
