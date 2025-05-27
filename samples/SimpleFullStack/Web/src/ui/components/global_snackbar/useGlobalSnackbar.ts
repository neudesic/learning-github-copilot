import { create } from 'zustand';
import type { SnackbarProps } from '@mui/material/Snackbar';
import type { AlertColor, AlertProps } from '@mui/material/Alert';

export type GlobalSnackbarStore = {
	isOpen: boolean;
	message: string;
	severity: AlertColor;
	snackbarProps?: Omit<
		SnackbarProps,
		'open' | 'onClose' | 'message' | 'children'
	>;
	alertProps?: Omit<AlertProps, 'severity' | 'children' | 'onClose'>;
	open: (
		message: string,
		severity: AlertColor,
		snackbarProps?: GlobalSnackbarStore['snackbarProps'],
		alertProps?: GlobalSnackbarStore['alertProps']
	) => void;
	close: () => void;
};

const useGlobalSnackbar = create<GlobalSnackbarStore>(set => ({
	isOpen: false,
	message: '',
	severity: 'success',
	snackbarProps: undefined,
	alertProps: undefined,
	open: (message, severity, snackbarProps, alertProps) =>
		set({ isOpen: true, message, severity, snackbarProps, alertProps }),
	close: () =>
		set({
			isOpen: false,
			message: '',
			severity: 'success',
			snackbarProps: undefined,
			alertProps: undefined,
		}),
}));

export default useGlobalSnackbar;
