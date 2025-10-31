import { useState } from 'react';
import { Card, CardContent, CardMedia, Typography, Box, Chip, Button, CardActions, IconButton, Collapse } from '@mui/material';
import type { Product } from 'types/Product';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ShoppingCartIcon from '@mui/icons-material/ShoppingCart';
import styled from '@emotion/styled';

interface ProductCardProps {
    product: Product;
    onViewDetails: (productId: number) => void;
}

const ExpandMore = styled(IconButton)<{ expanded: boolean }>(({ expanded }) => ({
    transform: expanded ? 'rotate(180deg)' : 'rotate(0deg)',
    transition: 'transform 0.3s',
}));

const ProductCard = ({ product, onViewDetails }: ProductCardProps) => {
    const [expanded, setExpanded] = useState(false);

    const handleExpandClick = () => {
        setExpanded(!expanded);
    };

    // Generate a placeholder image URL based on the product name
    const imageUrl = `https://source.unsplash.com/400x300/?${encodeURIComponent(product.name.split(' ')[0].toLowerCase())}`;

    return (
        <Card
            sx={{
                height: '100%',
                display: 'flex',
                flexDirection: 'column',
                transition: 'transform 0.2s, box-shadow 0.2s',
                '&:hover': {
                    transform: 'translateY(-4px)',
                    boxShadow: 6,
                }
            }}
        >
            <CardMedia
                component="img"
                height="140"
                image={imageUrl}
                alt={product.name}
            />
            <CardContent sx={{ flexGrow: 1 }}>
                <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start', mb: 1 }}>
                    <Typography gutterBottom variant="h6" component="div" sx={{ fontWeight: 'bold' }}>
                        {product.name}
                    </Typography>
                    {product.isActive ? (
                        <Chip size="small" label="Active" color="success" />
                    ) : (
                        <Chip size="small" label="Inactive" color="error" />
                    )}
                </Box>

                <Typography variant="body2" color="text.secondary" sx={{ mb: 1 }}>
                    {product.description?.substring(0, 100)}{product.description && product.description.length > 100 ? '...' : ''}
                </Typography>

                <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 0.5, mb: 1 }}>
                    <Chip
                        size="small"
                        label={product.category.name}
                        variant="outlined"
                        color="primary"
                    />
                    {product.brand && (
                        <Chip
                            size="small"
                            label={product.brand}
                            variant="outlined"
                        />
                    )}
                </Box>

                <Typography variant="caption" color="text.secondary">
                    SKU: {product.sku}
                </Typography>
            </CardContent>

            <CardActions sx={{ justifyContent: 'space-between', px: 2, pb: 1 }}>
                <Button
                    size="small"
                    variant="outlined"
                    onClick={() => onViewDetails(product.productID)}
                >
                    View Details
                </Button>

                <ExpandMore
                    expanded={expanded}
                    onClick={handleExpandClick}
                    aria-expanded={expanded}
                    aria-label="show more"
                >
                    <ExpandMoreIcon />
                </ExpandMore>
            </CardActions>

            <Collapse in={expanded} timeout="auto" unmountOnExit>
                <CardContent>
                    <Typography paragraph variant="subtitle2">Category Path:</Typography>
                    <Typography paragraph variant="body2">
                        {product.category.name}
                        {product.category.parentCategoryID && " (subcategory)"}
                    </Typography>

                    <Box sx={{ display: 'flex', justifyContent: 'flex-end' }}>
                        <Button
                            variant="contained"
                            color="primary"
                            startIcon={<ShoppingCartIcon />}
                            size="small"
                        >
                            Add to Cart
                        </Button>
                    </Box>
                </CardContent>
            </Collapse>
        </Card>
    );
};

export default ProductCard;
