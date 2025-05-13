import { create } from "zustand";

type AppState = {
  isDarkMode: boolean;
  toggleDarkMode: () => void;
};

export const useAppStore = create<AppState>((set) => ({
  isDarkMode: false,
  toggleDarkMode: () => set((state) => ({ isDarkMode: !state.isDarkMode })),
}));
