import React from 'react';
import { Breadcrumbs, capitalize, Link, Typography } from '@mui/material';
import { useLocation, Link as RouterLink } from 'react-router-dom';

const BreadCrumb: React.FC = () => {
    const location = useLocation();
    const pathnames = location.pathname.split('/').filter((x) => x);
    if (pathnames.length === 0) return null;
    return (
        <Breadcrumbs aria-label="breadcrumb" sx={{ mb: 2 }}>
            <Link
                underline="hover"
                color="inherit"
                component={RouterLink}
                to="/"
            >
                Home
            </Link>
            {pathnames.map((value, index) => {
                const to = `/${pathnames.slice(0, index + 1).join('/')}`;
                const isLast = index === pathnames.length - 1;
                return isLast ? (
                    <Typography color="text.primary" key={to}>
                        {capitalize(decodeURIComponent(value))}
                    </Typography>
                ) : (
                    <Link
                        underline="hover"
                        color="inherit"
                        component={RouterLink}
                        to={to}
                        key={to}
                    >
                        {capitalize(decodeURIComponent(value))}
                    </Link>
                );
            })}
        </Breadcrumbs>
    );
};

export default BreadCrumb;
