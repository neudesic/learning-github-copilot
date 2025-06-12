import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogTitle from '@mui/material/DialogTitle';
import IconButton from '@mui/material/IconButton';
import CloseIcon from '@mui/icons-material/Close';
import useGlobalModal from './useGlobalModal';

const GlobalModal = () => {
    const { isOpen, content, close, dialogProps, title, titleColor, primaryAction, primaryActionText, closeAction, formName } = useGlobalModal();

    const handleClose = () => {
        if (closeAction) {
            closeAction();
        } else {
            close();
        }
    };

    return (
        <Dialog {...(dialogProps || {})} open={isOpen} onClose={handleClose}>
            {title && (
                <DialogTitle
                    sx={{
                        display: 'flex',
                        justifyContent: 'space-between',
                        alignItems: 'center',
                        backgroundColor: titleColor ?? 'inherit',
                        position: 'sticky',
                        top: 0,
                        zIndex: 10,
                    }}
                >
                    {title}
                    <IconButton onClick={handleClose}>
                        <CloseIcon sx={{ color: 'white' }} />
                    </IconButton>
                </DialogTitle>
            )}
            <DialogContent sx={{ overflowY: 'auto', maxHeight: '60vh', marginTop: 3, pt: '6px !important' }}>{content}</DialogContent>
            <DialogActions sx={{ position: 'sticky', bottom: 0, backgroundColor: 'white', p: 2 }}>
                <Box sx={{ display: 'flex', justifyContent: 'flex-end', width: '100%' }}>
                    <Button variant="outlined" onClick={handleClose} sx={{ mr: 1 }}>
                        Cancel
                    </Button>
                    {formName && (
                        <Button variant="contained" color="primary" type="submit" form={formName}>
                            {primaryActionText ?? 'Save'}
                        </Button>
                    )}
                    {primaryAction && (
                        <Button variant="contained" onClick={primaryAction}>
                            {primaryActionText ?? 'Save'}
                        </Button>
                    )}
                </Box>
            </DialogActions>
        </Dialog>
    );
};

export default GlobalModal;
