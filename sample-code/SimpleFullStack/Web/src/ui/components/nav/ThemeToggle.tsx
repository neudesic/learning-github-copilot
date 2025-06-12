import React from 'react';
import { IconButton, Tooltip } from '@mui/material';
import LightModeIcon from '@mui/icons-material/LightMode';
import DarkModeIcon from '@mui/icons-material/DarkMode';
import useAppConfig from 'store/useAppConfig';
import { useTheme } from '@mui/material/styles';

const ThemeToggle: React.FC = () => {
    const theme = useAppConfig(state => state.theme);
    const setTheme = useAppConfig(state => state.setTheme);
    const isDarkMode = theme === 'dark';
    const muiTheme = useTheme();

    const iconColor = isDarkMode ? muiTheme.palette.warning.light : muiTheme.palette.primary.main;

    const toggleDarkMode = () => {
        setTheme(isDarkMode ? 'light' : 'dark');
    };

    return (
        <Tooltip title={isDarkMode ? 'Switch to Light Mode' : 'Switch to Dark Mode'}>
            <IconButton onClick={toggleDarkMode} sx={{ color: iconColor }}>
                {isDarkMode ? <DarkModeIcon /> : <LightModeIcon />}
            </IconButton>
        </Tooltip>
    );
};

export default ThemeToggle;