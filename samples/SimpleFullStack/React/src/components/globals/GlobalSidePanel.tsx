import { AppBar, Drawer, Typography } from '@mui/material';
import useGlobalSidePanel from 'store/global/useGlobalSidePanel';
import CloseIcon from '@mui/icons-material/Close';

const SidePanel = () => {
    const { isOpen, content, title, close } = useGlobalSidePanel();

    if (!isOpen) return null;

    if (!content) return null;

    return (
        <Drawer
            anchor={'right'}
            open={isOpen}
            onClose={close}
            sx={{ zIndex: 1300, fullheight: '100vh' }}
            PaperProps={{ sx: { width: 600, display: 'flex', fullheight: '100vh' } }}>
            <AppBar position="sticky" sx={{ p: 2, backgroundColor: 'primary.main', color: 'primary.contrastText' }}>
                <CloseIcon onClick={close} sx={{ position: 'absolute', top: 15, right: 10, cursor: 'pointer' }} />
                <Typography variant="h6" sx={{ fontWeight: 600 }}>
                    {title}
                </Typography>
            </AppBar>
            {content}
        </Drawer>
    );
}

export default SidePanel;


