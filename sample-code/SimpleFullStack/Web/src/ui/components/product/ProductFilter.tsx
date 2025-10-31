import { useState, useEffect } from 'react';
import {
    Box,
    TextField,
    FormControl,
    InputLabel,
    Select,
    MenuItem,
    Typography,
    Chip,
    IconButton,
    Paper,
    InputAdornment
} from '@mui/material';
import type { SelectChangeEvent } from '@mui/material/Select';
import SearchIcon from '@mui/icons-material/Search';
import FilterListIcon from '@mui/icons-material/FilterList';
import ClearIcon from '@mui/icons-material/Clear';
import type { Category } from 'types/Category';

interface ProductFilterProps {
    categories: Category[];
    onFilterChange: (filters: FilterState) => void;
}

export interface FilterState {
    searchTerm: string;
    categoryId: number | '';
    brand: string | '';
    activeOnly: boolean;
}

const ProductFilter = ({ categories, onFilterChange }: ProductFilterProps) => {
    const [filters, setFilters] = useState<FilterState>({
        searchTerm: '',
        categoryId: '',
        brand: '',
        activeOnly: false
    }); const [brands, setBrands] = useState<string[]>([]);

    // Get unique brands from product data - in a real app, this would come from an API
    useEffect(() => {
        // This would normally be an API call or derived from products
        setBrands(['Apple', 'Dell', 'Samsung', 'Sony', 'HP', 'MSI', 'Generic', 'GreenThumb']);
    }, []);

    // Get all categories including subcategories for the dropdown
    const getAllCategories = () => {
        const allCats: Category[] = [];

        const traverseCategories = (cats: Category[]) => {
            cats.forEach(cat => {
                allCats.push(cat);
                if (cat.subCategories && cat.subCategories.length > 0) {
                    traverseCategories(cat.subCategories);
                }
            });
        };

        traverseCategories(categories);
        return allCats;
    };

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        const newFilters = { ...filters, [name]: value };
        setFilters(newFilters);
        onFilterChange(newFilters);
    };

    const handleSelectChange = (e: SelectChangeEvent<string | number>) => {
        const { name, value } = e.target;
        const newFilters = { ...filters, [name]: value };
        setFilters(newFilters);
        onFilterChange(newFilters);
    };

    const handleClearFilters = () => {
        const clearedFilters = {
            searchTerm: '',
            categoryId: '',
            brand: '',
            activeOnly: false
        };
        setFilters(clearedFilters);
        onFilterChange(clearedFilters);
    };

    const toggleActiveOnly = () => {
        const newActiveOnly = !filters.activeOnly;
        const newFilters = { ...filters, activeOnly: newActiveOnly };
        setFilters(newFilters);
        onFilterChange(newFilters);
    };

    const allCategories = getAllCategories();

    const hasActiveFilters =
        filters.searchTerm !== '' ||
        filters.categoryId !== '' ||
        filters.brand !== '' ||
        filters.activeOnly;

    return (
        <Paper
            elevation={2}
            sx={{
                p: 2,
                mb: 3,
                borderRadius: 2
            }}
        >
            <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', mb: 2 }}>
                <Box sx={{ display: 'flex', alignItems: 'center' }}>
                    <FilterListIcon sx={{ mr: 1 }} />
                    <Typography variant="h6">Filters</Typography>
                </Box>

                {hasActiveFilters && (
                    <IconButton size="small" onClick={handleClearFilters} title="Clear all filters">
                        <ClearIcon fontSize="small" />
                    </IconButton>
                )}
            </Box>

            <Box sx={{ display: 'flex', flexDirection: { xs: 'column', sm: 'row' }, gap: 2, mb: 2 }}>
                <TextField
                    fullWidth
                    name="searchTerm"
                    label="Search Products"
                    variant="outlined"
                    value={filters.searchTerm}
                    onChange={handleInputChange}
                    size="small"
                    InputProps={{
                        startAdornment: (
                            <InputAdornment position="start">
                                <SearchIcon />
                            </InputAdornment>
                        ),
                        endAdornment: filters.searchTerm ? (
                            <InputAdornment position="end">
                                <IconButton
                                    size="small"
                                    onClick={() => {
                                        const newFilters = { ...filters, searchTerm: '' };
                                        setFilters(newFilters);
                                        onFilterChange(newFilters);
                                    }}
                                >
                                    <ClearIcon fontSize="small" />
                                </IconButton>
                            </InputAdornment>
                        ) : null
                    }}
                />

                <FormControl size="small" fullWidth>
                    <InputLabel id="category-label">Category</InputLabel>
                    <Select
                        labelId="category-label"
                        name="categoryId"
                        value={filters.categoryId}
                        onChange={handleSelectChange}
                        label="Category"
                    >
                        <MenuItem value="">
                            <em>All Categories</em>
                        </MenuItem>
                        {allCategories.map((category) => (
                            <MenuItem key={category.categoryID} value={category.categoryID}>
                                {category.name}
                            </MenuItem>
                        ))}
                    </Select>
                </FormControl>
            </Box>

            <Box sx={{ display: 'flex', flexDirection: { xs: 'column', sm: 'row' }, gap: 2 }}>
                <FormControl size="small" fullWidth>
                    <InputLabel id="brand-label">Brand</InputLabel>
                    <Select
                        labelId="brand-label"
                        name="brand"
                        value={filters.brand}
                        onChange={handleSelectChange}
                        label="Brand"
                    >
                        <MenuItem value="">
                            <em>All Brands</em>
                        </MenuItem>
                        {brands.map((brand) => (
                            <MenuItem key={brand} value={brand}>
                                {brand}
                            </MenuItem>
                        ))}
                    </Select>
                </FormControl>

                <Box
                    sx={{
                        display: 'flex',
                        alignItems: 'center',
                        cursor: 'pointer',
                        border: 1,
                        borderColor: 'divider',
                        borderRadius: 1,
                        p: 1,
                        minWidth: 120,
                        justifyContent: 'center'
                    }}
                    onClick={toggleActiveOnly}
                >
                    <Chip
                        label="Active Only"
                        color={filters.activeOnly ? "primary" : "default"}
                        variant={filters.activeOnly ? "filled" : "outlined"}
                        sx={{ width: '100%' }}
                    />
                </Box>
            </Box>

            {hasActiveFilters && (
                <Box sx={{ mt: 2, pt: 2, borderTop: '1px dashed #ccc' }}>
                    <Typography variant="subtitle2" sx={{ mb: 1 }}>Active Filters:</Typography>
                    <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 1 }}>
                        {filters.searchTerm && (
                            <Chip
                                size="small"
                                label={`Search: ${filters.searchTerm}`}
                                onDelete={() => {
                                    const newFilters = { ...filters, searchTerm: '' };
                                    setFilters(newFilters);
                                    onFilterChange(newFilters);
                                }}
                            />
                        )}

                        {filters.categoryId !== '' && (
                            <Chip
                                size="small"
                                label={`Category: ${allCategories.find(c => c.categoryID === filters.categoryId)?.name}`}
                                onDelete={() => {
                                    const newFilters = { ...filters, categoryId: '' };
                                    setFilters(newFilters);
                                    onFilterChange(newFilters);
                                }}
                            />
                        )}

                        {filters.brand !== '' && (
                            <Chip
                                size="small"
                                label={`Brand: ${filters.brand}`}
                                onDelete={() => {
                                    const newFilters = { ...filters, brand: '' };
                                    setFilters(newFilters);
                                    onFilterChange(newFilters);
                                }}
                            />
                        )}

                        {filters.activeOnly && (
                            <Chip
                                size="small"
                                label="Active Only"
                                onDelete={toggleActiveOnly}
                            />
                        )}
                    </Box>
                </Box>
            )}
        </Paper>
    );
};

export default ProductFilter;
