import Box from '@mui/material/Box';
import Footer from 'ui/components/footer/Footer';
import { Outlet } from 'react-router-dom';
import NavBar from '../nav/NavBar';


const MainLayout = () => {
    return (
        <Box
            sx={{
                minHeight: '100vh',
                bgcolor: 'background.default',
                width: '100%',
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
            }}
        >
            <NavBar />
            <Box sx={{ flex: 1, width: '100%', maxWidth: 1200, display: 'flex' }}>
                <Outlet />
            </Box>
            <Footer />
        </Box>
    );
};

export default MainLayout;
