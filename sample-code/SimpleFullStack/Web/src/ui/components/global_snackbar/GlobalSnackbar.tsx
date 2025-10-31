import React from 'react';
import Snackbar from '@mui/material/Snackbar';
import Alert from '@mui/material/Alert';
import Slide from '@mui/material/Slide';
import type { SlideProps } from '@mui/material/Slide';
import useGlobalSnackbar from './useGlobalSnackbar';

function TransitionUp(props: SlideProps) {
  return <Slide {...props} direction="up" />;
}

export default function GlobalSnackbar() {
  const { isOpen, message, severity, close, snackbarProps, alertProps } = useGlobalSnackbar();

  const handleClose = (_event?: React.SyntheticEvent | Event, reason?: string) => {
    if (reason === 'clickaway') return;
    close();
  };

  return (
    <Snackbar
      open={isOpen}
      onClose={handleClose}
      message={message}
      autoHideDuration={3000}
      slots={{ transition: TransitionUp }}
      {...(snackbarProps || {})}
    >
      <Alert
        severity={severity}
        sx={{ minWidth: '300px', borderRadius: '10px' }}
        onClose={handleClose}
        {...(alertProps || {})}
      >
        {message}
      </Alert>
    </Snackbar>
  );
}
