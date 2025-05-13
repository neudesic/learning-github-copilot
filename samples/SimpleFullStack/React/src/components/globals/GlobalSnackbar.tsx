import {
  Snackbar,
  Alert,
  Slide,
  SlideProps,
  SnackbarOrigin,
} from '@mui/material';
import useSnackbar from '../../store/global/useGlobalSnackbar';

function TransitionUp(props: JSX.IntrinsicAttributes & SlideProps) {
  return <Slide {...props} direction="up" />;
}

export default function GlobalSnackbar() {
  const { isOpen, message, severity, vertical, horizontal, close } =
    useSnackbar();

  const handleClose = () => {
    close();
  };

  return (
    <Snackbar
      anchorOrigin={
        {
          vertical,
          horizontal,
        } as SnackbarOrigin
      }
      open={isOpen}
      autoHideDuration={5000}
      onClose={handleClose}
      message={message}
      TransitionComponent={TransitionUp}
    >
      <Alert
        severity={severity}
        sx={{ minWidth: '300px', borderRadius: '10px' }}
      >
        {message}
      </Alert>
    </Snackbar>
  );
}
