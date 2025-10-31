import React, { Component, type ReactNode } from 'react';
import { Box, Typography, Button } from '@mui/material';

interface ErrorBoundaryProps {
    children: ReactNode;
}

interface ErrorBoundaryState {
    hasError: boolean;
    error?: Error;
}

class ErrorBoundary extends Component<ErrorBoundaryProps, ErrorBoundaryState> {
    constructor(props: ErrorBoundaryProps) {
        super(props);
        this.state = { hasError: false };
    }

    static getDerivedStateFromError(error: Error) {
        return { hasError: true, error };
    }

    componentDidCatch(error: Error, errorInfo: React.ErrorInfo) {
        console.error('ErrorBoundary caught an error:', error, errorInfo);
    }

    handleReload = () => {
        window.location.reload();
    };

    render() {
        if (this.state.hasError) {
            return (
                <Box sx={{ p: 4, textAlign: 'center' }}>
                    <Typography variant="h4" color="error" gutterBottom>
                        Something went wrong.
                    </Typography>
                    <Typography variant="body1" gutterBottom>
                        {this.state.error?.message || 'An unexpected error occurred.'}
                    </Typography>
                    <Button variant="contained" color="primary" onClick={this.handleReload}>
                        Reload Page
                    </Button>
                </Box>
            );
        }
        return this.props.children;
    }
}

export default ErrorBoundary;
