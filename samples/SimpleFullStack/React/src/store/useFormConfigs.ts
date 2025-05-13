import { create } from 'zustand';

type FormConfigStore = {
  isDirty: boolean;
  setIsDirty: (isDirty: boolean) => void;
};

export const useFormConfig = create<FormConfigStore>(set => ({
  isDirty: false,
  setIsDirty: isDirty => set({ isDirty }),
}));
