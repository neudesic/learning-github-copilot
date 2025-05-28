import { useState, useEffect } from 'react';
import { useParams, Link as RouterLink } from 'react-router-dom';
import {
    Container,
    Grid,
    Typography,
    Box,
    Paper,
    Chip,
    Divider,
    Button,
    CircularProgress,
    Breadcrumbs,
    Link,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableRow
} from '@mui/material';
import HomeIcon from '@mui/icons-material/Home';
import ShoppingBasketIcon from '@mui/icons-material/ShoppingBasket';
import ShoppingCartIcon from '@mui/icons-material/ShoppingCart';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import type { Product } from 'types/Product';
import type { ProductAttribute } from 'types/ProductAttribute';
import productsData from 'json/products.json';
import productAttributesData from 'json/productAttributes.json';
import { APPLICATION } from 'config/constants';

const ProductDetailPage = () => {
    const { productId } = useParams<{ productId: string }>();
    const [product, setProduct] = useState<Product | null>(null);
    const [attributes, setAttributes] = useState<ProductAttribute[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const loadProductData = async () => {
            try {
                setLoading(true);
                // Simulate API delay
                await new Promise(resolve => setTimeout(resolve, 800));

                // Find the product by ID
                const productId_num = Number(productId);
                const foundProduct = (productsData as unknown as Product[]).find(p => p.productID === productId_num);

                if (foundProduct) {
                    setProduct(foundProduct);

                    // Find the product attributes
                    const productAttributes = (productAttributesData as unknown as ProductAttribute[]).filter(
                        attr => attr.productID === productId_num
                    );

                    setAttributes(productAttributes);
                }
            } catch (error) {
                console.error('Error loading product details:', error);
            } finally {
                setLoading(false);
            }
        };

        if (productId) {
            loadProductData();
        }
    }, [productId]);

    if (loading) {
        return (
            <Container sx={{ py: 8, textAlign: 'center' }}>
                <CircularProgress />
                <Typography variant="h6" sx={{ mt: 2 }}>
                    Loading product details...
                </Typography>
            </Container>
        );
    }

    if (!product) {
        return (
            <Container sx={{ py: 8, textAlign: 'center' }}>
                <Typography variant="h5" color="error" gutterBottom>
                    Product not found
                </Typography>
                <Button
                    component={RouterLink}
                    to={APPLICATION.Products.route}
                    startIcon={<ArrowBackIcon />}
                    variant="contained"
                    sx={{ mt: 2 }}
                >
                    Back to Products
                </Button>
            </Container>
        );
    }

    // Generate a placeholder image URL based on the product name
    const imageUrl = `https://source.unsplash.com/800x600/?${encodeURIComponent(product.name.split(' ')[0].toLowerCase())}`;

    return (
        <Container maxWidth="lg" sx={{ py: 4 }}>
            {/* Breadcrumbs */}
            <Breadcrumbs aria-label="breadcrumb" sx={{ mb: 3 }}>
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
                <Link
                    component={RouterLink}
                    to={APPLICATION.Products.route}
                    underline="hover"
                    color="inherit"
                    sx={{ display: 'flex', alignItems: 'center' }}
                >
                    <ShoppingBasketIcon sx={{ mr: 0.5 }} fontSize="inherit" />
                    Products
                </Link>
                <Typography color="text.primary">{product.name}</Typography>
            </Breadcrumbs>

            <Button
                component={RouterLink}
                to={APPLICATION.Products.route}
                startIcon={<ArrowBackIcon />}
                sx={{ mb: 3 }}
            >
                Back to Products
            </Button>

            <Grid container spacing={4}>
                {/* Product Image */}
                <Grid item xs={12} md={6}>
                    <Paper
                        elevation={2}
                        sx={{
                            p: 2,
                            height: '100%',
                            borderRadius: 2,
                            overflow: 'hidden'
                        }}
                    >
                        <Box
                            component="img"
                            src={imageUrl}
                            alt={product.name}
                            sx={{
                                width: '100%',
                                height: 'auto',
                                objectFit: 'cover',
                                borderRadius: 1
                            }}
                        />
                    </Paper>
                </Grid>

                {/* Product Details */}
                <Grid item xs={12} md={6}>
                    <Paper
                        elevation={2}
                        sx={{
                            p: 3,
                            height: '100%',
                            borderRadius: 2,
                            display: 'flex',
                            flexDirection: 'column'
                        }}
                    >
                        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 1 }}>
                            <Typography variant="h4" component="h1" gutterBottom>
                                {product.name}
                            </Typography>
                            {product.isActive ? (
                                <Chip label="Active" color="success" />
                            ) : (
                                <Chip label="Inactive" color="error" />
                            )}
                        </Box>

                        <Box sx={{ display: 'flex', mb: 2, gap: 1, flexWrap: 'wrap' }}>
                            <Chip
                                label={product.category.name}
                                color="primary"
                                variant="outlined"
                            />
                            {product.brand && <Chip label={product.brand} variant="outlined" />}
                        </Box>

                        <Typography variant="subtitle1" color="text.secondary" paragraph>
                            SKU: {product.sku}
                        </Typography>

                        <Divider sx={{ my: 2 }} />

                        <Typography variant="h6" gutterBottom>Description</Typography>
                        <Typography variant="body1" paragraph>
                            {product.description || "No description available."}
                        </Typography>

                        <Divider sx={{ my: 2 }} />

                        <Box sx={{ mt: 'auto' }}>
                            <Button
                                variant="contained"
                                color="primary"
                                size="large"
                                startIcon={<ShoppingCartIcon />}
                                fullWidth
                                sx={{ mt: 2 }}
                            >
                                Add to Cart
                            </Button>
                        </Box>
                    </Paper>
                </Grid>

                {/* Product Attributes */}
                <Grid item xs={12}>
                    <Paper elevation={2} sx={{ p: 3, borderRadius: 2 }}>
                        <Typography variant="h5" gutterBottom>Specifications</Typography>

                        {attributes.length > 0 ? (
                            <TableContainer>
                                <Table>
                                    <TableBody>
                                        {attributes.map((attr) => (
                                            <TableRow key={attr.attributeID}>
                                                <TableCell
                                                    component="th"
                                                    scope="row"
                                                    sx={{
                                                        fontWeight: 'bold',
                                                        width: '30%',
                                                        borderBottom: '1px solid rgba(224, 224, 224, 0.5)'
                                                    }}
                                                >
                                                    {attr.attributeName}
                                                </TableCell>
                                                <TableCell
                                                    sx={{
                                                        borderBottom: '1px solid rgba(224, 224, 224, 0.5)'
                                                    }}
                                                >
                                                    {attr.attributeValue}
                                                </TableCell>
                                            </TableRow>
                                        ))}
                                    </TableBody>

                                </Table>
                            </TableContainer>
                        ) : (
                            <Typography variant="body1" color="text.secondary">
                                No specifications available for this product.
                            </Typography>
                        )}
                    </Paper>
                </Grid>

                {/* Add a section */}




            </Grid>
        </Container>
    );
};

export default ProductDetailPage;
