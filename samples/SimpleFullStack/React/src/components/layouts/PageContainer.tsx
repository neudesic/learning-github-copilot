import { Outlet } from 'react-router-dom';
import LogoutIcon from '@mui/icons-material/Logout';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Grid from '@mui/material/Grid';
import IconButton from '@mui/material/IconButton';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import NeudesicLogo from 'assets/Neudesic_Logo.svg';

export const PageContainer = () => {
  const handleLogout = () => {
  };

  return (
    <Box
      sx={{
        display: 'flex',
        justifyContent: 'center',
        flexFlow: 'column',
        minHeight: '100vh',
        alignItems: 'center',
      }}
    >
      <AppBar
        position="relative"
        sx={{
          background:
            'radial-gradient(55.41% 122.41% at 50% 30%, #208EBF 0%, #0F4259 100%)',
          borderBottom: '1px solid #FFF',
          boxShadow: 'none',
        }}
      >
        <Toolbar
          sx={{
            py: '20px !important',
            px: '30px !important',
            justifyContent: 'space-between',
          }}
        >
          <Typography variant="h4" color={"common.white"} component="div">React Base </Typography>
          <IconButton onClick={handleLogout} children={<LogoutIcon />} />
        </Toolbar>
      </AppBar>
      <main
        style={{
          display: 'flex',
          flexDirection: 'column',
          maxWidth: '1920px',
          padding: '30px',
          width: '100%',
          flexGrow: 1,
        }}
      >
        <Box
          sx={{
            display: 'flex',
            flexDirection: 'column',
            gap: 3,
            width: '100%',
            overflowY: 'auto',
          }}
        >
          <Outlet />
        </Box>
      </main>
      <Grid
        container
        sx={{
          background: "main.primary",
          borderTop: '1px solid #FFF',
          boxShadow: 'none',
          width: '100%',
          justifyContent: 'space-between',
          alignItems: 'center',
          p: '17px 30px',
        }}
      >
        <Typography variant="caption">
          © 2024 Neudesic, LLC, All Rights Reserved
        </Typography>
        <img src={NeudesicLogo} alt="Neudesic Logo" width="117px" />
      </Grid>
    </Box>
  );
};
