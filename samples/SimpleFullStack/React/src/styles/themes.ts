import { createTheme } from '@mui/material/styles';

// Theme Types: Custom colors
export interface CustomPaletteOptions {
  darkGrey: string;
  white: string;
  dodgerBlue: string;
}

export const theme = createTheme({
  spacing: 8,
  palette: {
    mode: 'light',
    custom: {
      darkGrey: '#8C8C8C',
      white: '#FFFFFF',
      dodgerBlue: '#2196F3',
    },
    primary: {
      main: '#00467F',
      light: '#027AA9',
    },
    warning: {
      main: '#F4802B',
    },
    grey: {
      400: '#BDBDBD',
    },
  },
  components: {
    MuiDialogTitle: {
      styleOverrides: {
        root: ({ theme }) => ({
          backgroundColor: theme.palette.primary.main, // Use theme primary color
          color: theme.palette.primary.contrastText, // Use theme white color
        }),
      },
    },
    MuiAppBar: {
      styleOverrides: {
        root: ({ theme }) => ({
          backgroundColor: theme.palette.background.default, // Use theme primary color
          color: theme.palette.text.primary, // Use theme white color
        }),
      },
    },
    MuiButton: {
      styleOverrides: {
        contained: {
          '&:hover': {
            background: '#027AA9',
            color: '#FFFFFF',
          },
        },
        text: {
          color: '#2196F3',
          textTransform: 'none',
        },
      },
    },
  },
});
