import { useState, useEffect } from 'react';
import { useNavigate, Link as RouterLink } from 'react-router-dom';
import {
    Box,
    Typography,
    Container,
    CircularProgress,
    Breadcrumbs,
    Link,
    Button,
    Divider
} from '@mui/material';
import ProductFilter from 'ui/components/product/ProductFilter';
import type { FilterState } from 'ui/components/product/ProductFilter';
import ProductsGrid from 'ui/components/product/ProductsGrid';
import AddIcon from '@mui/icons-material/Add';
import HomeIcon from '@mui/icons-material/Home';
import type { Product } from 'types/Product';
import type { Category } from 'types/Category';
import productsData from 'json/products.json';
import categoriesData from 'json/categories.json';
import { APPLICATION } from 'config/constants';

const ProductsPages = () => {
    const navigate = useNavigate();
    const [loading, setLoading] = useState(true);
    const [products, setProducts] = useState<Product[]>([]);
    const [filteredProducts, setFilteredProducts] = useState<Product[]>([]);
    const [categories, setCategories] = useState<Category[]>([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const itemsPerPage = 9; useEffect(() => {
        // In a real app, this would be an API call
        const loadData = async () => {
            try {
                setLoading(true);
                // Simulate API delay
                await new Promise(resolve => setTimeout(resolve, 800));

                // Use unknown as intermediary type for safer type assertion
                setProducts(productsData as unknown as Product[]);
                setCategories(categoriesData as unknown as Category[]);
                setFilteredProducts(productsData as unknown as Product[]);

                const totalPages = Math.ceil(productsData.length / itemsPerPage);
                setTotalPages(totalPages);
            } catch (error) {
                console.error('Error loading products:', error);
            } finally {
                setLoading(false);
            }
        };

        loadData();
    }, []);

    const handleFilterChange = (filters: FilterState) => {
        // Filter products based on filter criteria
        let filtered = [...products];

        if (filters.searchTerm) {
            const searchLower = filters.searchTerm.toLowerCase();
            filtered = filtered.filter(product =>
                product.name.toLowerCase().includes(searchLower) ||
                (product.description && product.description.toLowerCase().includes(searchLower)) ||
                (product.brand && product.brand.toLowerCase().includes(searchLower)) ||
                product.sku.toLowerCase().includes(searchLower)
            );
        }

        if (filters.categoryId !== '') {
            filtered = filtered.filter(product => {
                // Check if the product is in this category or any of its subcategories
                const categoryId = Number(filters.categoryId);
                return product.categoryID === categoryId;
            });
        }

        if (filters.brand !== '') {
            filtered = filtered.filter(product =>
                product.brand === filters.brand
            );
        }

        if (filters.activeOnly) {
            filtered = filtered.filter(product => product.isActive);
        }

        setFilteredProducts(filtered);
        setCurrentPage(1);
        setTotalPages(Math.ceil(filtered.length / itemsPerPage));
    };
    const handleViewDetails = (productId: number) => {
        // Navigate to product details page
        navigate(`${APPLICATION.Products.route}/${productId}`);
    };

    const handlePageChange = (page: number) => {
        setCurrentPage(page);
        // Scroll to top when changing pages
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    };

    // Get current page products
    const getCurrentPageProducts = () => {
        const startIndex = (currentPage - 1) * itemsPerPage;
        const endIndex = startIndex + itemsPerPage;
        return filteredProducts.slice(startIndex, endIndex);
    };

    if (loading) {
        return (
            <Container sx={{ py: 8, textAlign: 'center' }}>
                <CircularProgress />
                <Typography variant="h6" sx={{ mt: 2 }}>
                    Loading products...
                </Typography>
            </Container>
        );
    }

    return (
        <Container maxWidth="lg" sx={{ py: 4 }}>
            {/* Breadcrumbs */}      <Breadcrumbs aria-label="breadcrumb" sx={{ mb: 3 }}>
                <Link
                    component={RouterLink}
                    to="/"
                    underline="hover"
                    color="inherit"
                    sx={{ display: 'flex', alignItems: 'center' }}
                >
                    <HomeIcon sx={{ mr: 0.5 }} fontSize="inherit" />
                    Home
                </Link>
                <Typography color="text.primary">Products</Typography>
            </Breadcrumbs>

            {/* Header with title and add button */}
            <Box sx={{
                display: 'flex',
                justifyContent: 'space-between',
                alignItems: 'center',
                mb: 3
            }}>
                <Typography variant="h4" component="h1" gutterBottom>
                    Products
                    <Typography
                        component="span"
                        variant="subtitle1"
                        sx={{ ml: 2, color: 'text.secondary' }}
                    >
                        ({filteredProducts.length} items)
                    </Typography>
                </Typography>

                <Button
                    variant="contained"
                    color="primary"
                    startIcon={<AddIcon />}
                    onClick={() => alert('Add product clicked')}
                >
                    Add Product
                </Button>
            </Box>

            <Divider sx={{ mb: 3 }} />

            {/* Filters */}
            <ProductFilter
                categories={categories}
                onFilterChange={handleFilterChange}
            />

            {/* Products grid */}
            <ProductsGrid
                products={getCurrentPageProducts()}
                onViewDetails={handleViewDetails}
                page={currentPage}
                totalPages={totalPages}
                onPageChange={handlePageChange}
            />
        </Container>
    );
};

export default ProductsPages;