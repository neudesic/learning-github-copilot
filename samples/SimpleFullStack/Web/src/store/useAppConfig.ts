import { create } from 'zustand';

export type ThemeMode = 'light' | 'dark';

interface AppConfigStore {
	theme: ThemeMode;
	setTheme: (theme: ThemeMode) => void;
}

const useAppConfig = create<AppConfigStore>(set => ({
	theme: 'light',
	setTheme: theme => set({ theme }),
}));

export default useAppConfig;
