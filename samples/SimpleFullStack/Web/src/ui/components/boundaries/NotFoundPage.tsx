import React from 'react';
import { Box, Typography } from '@mui/material';

const NotFoundPage: React.FC = () => (
    <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', minHeight: '60vh' }}>
        <Typography variant="h3" color="error" gutterBottom>
            404
        </Typography>
        <Typography variant="h5" gutterBottom>
            Page Not Found
        </Typography>
        <Typography variant="body1">
            Sorry, the page you are looking for does not exist.
        </Typography>
    </Box>
);

export default NotFoundPage;
