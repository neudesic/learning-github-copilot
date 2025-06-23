// @ts-nocheck
// eslint-disable
import { Grid, Box, Typography, Pagination } from '@mui/material';
import ProductCard from './ProductCard';
import type { Product } from 'types/Product';

interface ProductsGridProps {
    products: Product[];
    onViewDetails: (productId: number) => void;
    page: number;
    totalPages: number;
    onPageChange: (page: number) => void;
}

const ProductsGrid = ({
    products,
    onViewDetails,
    page,
    totalPages,
    onPageChange
}: ProductsGridProps) => {
    if (products.length === 0) {
        return (
            <Box sx={{ py: 10, textAlign: 'center' }}>
                <Typography variant="h5" color="text.secondary" gutterBottom>
                    No products found
                </Typography>
                <Typography variant="body1" color="text.secondary">
                    Try adjusting your filters or search criteria
                </Typography>
            </Box>
        );
    }

    return (
        <Box sx={{ width: '100%' }}>
            <Grid container spacing={3}>
                {products.map((product) => (
                    <Grid item key={product.productID} xs={12} sm={6} md={4}>
                        <ProductCard
                            product={product}
                            onViewDetails={onViewDetails}
                        />
                    </Grid>
                ))}
            </Grid>

            {totalPages > 1 && (
                <Box sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
                    <Pagination
                        count={totalPages}
                        page={page}
                        onChange={(_, value) => onPageChange(value)}
                        color="primary"
                        showFirstButton
                        showLastButton
                    />
                </Box>
            )}
        </Box>
    );
};

export default ProductsGrid;