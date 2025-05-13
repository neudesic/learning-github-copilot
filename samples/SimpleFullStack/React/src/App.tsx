import { ThemeProvider } from '@emotion/react';
import { CssBaseline } from '@mui/material';
import { Suspense } from 'react';
import GlobalModal from 'components/globals/GlobalModal';
import GlobalSnackbar from 'components/globals/GlobalSnackbar';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import './global.css';
import GlobalSidePanel from 'components/globals/GlobalSidePanel';
import GlobalSecondaryModal from 'components/globals/GlobalSecondaryModal';

import { myRoutes } from './routes/privateRoutes';
import { theme } from './styles/themes';


function App() {

  return (
    <ThemeProvider theme={theme}>
      <LocalizationProvider dateAdapter={AdapterDayjs}>
        <Suspense fallback={<div>Global Loading...</div>}>
          <CssBaseline />
          <GlobalModal />
          <GlobalSecondaryModal />
          <GlobalSnackbar />
          <GlobalSidePanel />
          <RouterProvider router={createBrowserRouter(myRoutes)} />
        </Suspense>
      </LocalizationProvider>
    </ThemeProvider>
  );
}

export default App;
