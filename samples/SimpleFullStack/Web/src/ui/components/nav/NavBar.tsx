import React from 'react';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import IconButton from '@mui/material/IconButton';
import MenuIcon from '@mui/icons-material/Menu';
import Menu from '@mui/material/Menu';
import MenuItem from '@mui/material/MenuItem';
import Button from '@mui/material/Button';
import { Link as RouterLink, useLocation } from 'react-router-dom';
import { USER_NAME } from 'config/constants';
import useMediaQuery from '@mui/material/useMediaQuery';
import { useTheme } from '@mui/material/styles';
import { APPLICATION_ARRAY } from 'config/constants';
import ThemeToggle from './ThemeToggle';
import LightModeIcon from '@mui/icons-material/LightMode';
import DarkModeIcon from '@mui/icons-material/DarkMode';
import useAppConfig from 'store/useAppConfig';

const NavBar: React.FC = () => {
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));
    const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
    const location = useLocation();
    const [scrolled, setScrolled] = React.useState(false);
    const navLinks = APPLICATION_ARRAY;
    const themeStore = useAppConfig();
    const isDarkMode = themeStore.theme === 'dark';
    const setTheme = themeStore.setTheme;

    React.useEffect(() => {
        const handleScroll = () => {
            setScrolled(window.scrollY > 10);
        };
        window.addEventListener('scroll', handleScroll);
        return () => window.removeEventListener('scroll', handleScroll);
    }, []);

    const handleMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };
    const handleMenuClose = () => {
        setAnchorEl(null);
    };

    return (
        <AppBar
            position="sticky"
            color="transparent"
            elevation={scrolled ? 4 : 0}
            sx={{
                mb: 2,
                transition: 'background-color 0.3s, box-shadow 0.3s',
                backgroundColor: scrolled ? (theme.palette.background.paper + 'F2' || '#fff') : 'transparent',
                backdropFilter: scrolled ? 'blur(8px)' : undefined,
                boxShadow: scrolled ? theme.shadows[4] : 'none',
                overflow: 'hidden',
            }}
        >
            <Toolbar sx={{ px: { xs: 1, sm: 2 } }}>
                <Box sx={{ display: 'flex', alignItems: 'center', flexShrink: 0 }}>
                    <Typography
                        variant="h5"
                        component={RouterLink}
                        to={navLinks[0].route}
                        sx={{
                            fontWeight: 700,
                            color: 'primary.main',
                            textDecoration: 'none',
                            letterSpacing: 1,
                            mr: 2,
                        }}
                    >
                        {USER_NAME}
                    </Typography>
                </Box>
                <Box sx={{ flexGrow: 1 }} />
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                    <Box sx={{ display: { xs: 'none', md: 'flex' }, alignItems: 'center', gap: 2 }}>
                        {navLinks.map(link => (
                            <Button
                                key={link.route}
                                component={RouterLink}
                                to={link.route}
                                color={location.pathname === link.route ? 'primary' : 'inherit'}
                                startIcon={link.icon ? <link.icon fontSize="small" /> : undefined}
                                sx={{ fontWeight: 500, textTransform: 'none' }}
                            >
                                {link.label}
                            </Button>
                        ))}
                        <ThemeToggle />
                    </Box>
                    {isMobile && (
                        <IconButton
                            edge="end"
                            color="inherit"
                            aria-label="menu"
                            onClick={handleMenuOpen}
                            sx={{ ml: 1 }}
                        >
                            <MenuIcon />
                        </IconButton>
                    )}
                </Box>
            </Toolbar>
            {isMobile && (
                <Menu
                    anchorEl={anchorEl}
                    open={Boolean(anchorEl)}
                    onClose={handleMenuClose}
                    anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
                    transformOrigin={{ vertical: 'top', horizontal: 'right' }}
                    PaperProps={{
                        sx: {
                            minWidth: 180,
                            borderRadius: 2,
                            boxShadow: 4,
                            bgcolor: theme.palette.background.paper,
                            p: 1,
                        },
                    }}
                >
                    {navLinks.map(link => (
                        <MenuItem
                            key={link.route}
                            component={RouterLink}
                            to={link.route}
                            selected={location.pathname === link.route}
                            onClick={handleMenuClose}
                            sx={{
                                display: 'flex',
                                alignItems: 'center',
                                gap: 1.5,
                                borderRadius: 1,
                                fontWeight: 500,
                                color: location.pathname === link.route ? 'primary.main' : 'text.primary',
                                bgcolor: location.pathname === link.route ? 'action.selected' : 'transparent',
                                '&:hover': {
                                    bgcolor: 'action.hover',
                                },
                                px: 2,
                                py: 1.2,
                            }}
                        >
                            {link.icon && <span style={{ display: 'flex', alignItems: 'center' }}><link.icon fontSize="small" /></span>}
                            <span>{link.label}</span>
                        </MenuItem>
                    ))}
                    {/* Theme toggle menu item for mobile */}
                    <MenuItem
                        onClick={() => {
                            setTheme(isDarkMode ? 'light' : 'dark');
                            handleMenuClose();
                        }}
                        sx={{
                            display: 'flex',
                            alignItems: 'center',
                            gap: 1.5,
                            borderRadius: 1,
                            fontWeight: 500,
                            px: 2,
                            py: 1.2,
                        }}
                    >
                        <span style={{ display: 'flex', alignItems: 'center' }}>
                            {isDarkMode ? (
                                <DarkModeIcon fontSize="small" color="primary" />
                            ) : (
                                <LightModeIcon fontSize="small" color="primary" />
                            )}
                        </span>
                        <span style={{
                            fontSize: 15,
                            color: isDarkMode ? theme.palette.primary.light : theme.palette.primary.dark,
                            fontWeight: 500,
                        }}>
                            {isDarkMode ? 'Dark' : 'Light'} Mode
                        </span>
                    </MenuItem>
                </Menu>
            )}
        </AppBar>
    );
};

export default NavBar;
