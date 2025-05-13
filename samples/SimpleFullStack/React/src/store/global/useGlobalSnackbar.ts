import { AlertColor } from '@mui/material';
import { create } from 'zustand';

type GlobalSnackbarStore = {
  isOpen: boolean;
  message: string;
  severity: AlertColor;
  vertical?: string;
  horizontal?: string;

  open: (
    message: string,
    severity: AlertColor,
    vertical?: string,
    horizonal?: string,
  ) => void;
  close: () => void;
};

const useGlobalSnackbar = create<GlobalSnackbarStore>(set => ({
  isOpen: false,
  message: '',
  severity: 'success',
  vertical: 'bottom',
  horizontal: 'center',

  open: (message, severity, vertical = 'bottom', horizontal = 'center') =>
    set({ isOpen: true, message, severity, vertical, horizontal }),
  close: () => set({ isOpen: false, message: '', severity: 'success' }),
}));

export default useGlobalSnackbar;
