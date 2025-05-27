import React from 'react';
import { Box, Typography, CircularProgress } from '@mui/material';

const LoadingPage: React.FC = () => (
    <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', minHeight: '60vh' }}>
        <CircularProgress color="primary" sx={{ mb: 2 }} />
        <Typography variant="h5" color="primary" gutterBottom>
            Loading...
        </Typography>
    </Box>
);

export default LoadingPage;
