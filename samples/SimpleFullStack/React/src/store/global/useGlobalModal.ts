import React from 'react';
import { create } from 'zustand';
import { DialogProps } from '@mui/material/Dialog';
import { theme } from '../../styles/themes';

export type CustomModalProps = {
  title?: string;
  titleColor?: string;
  primaryAction?: () => void;
  primaryActionText?: string;
  closeAction?: () => void;
  formName?: string; // used to trigger form submit from outside form
};

export type OpenModalProps = CustomModalProps & {
  content: React.ReactNode;
  additionalActions?: React.ReactNode;
  dialogProps?: Partial<DialogProps>; // Allow custom MUI Dialog props
  isDirtyCheck?: boolean; // Check for unsaved changes
};

type GlobalModalStore = {
  state: {
    content: React.ReactNode | null;
    additionalActions: React.ReactNode | null;
    dialogProps: DialogProps; // Now holds isOpen and all Dialog properties
    isDirtyCheck: boolean;
  } & CustomModalProps;
  openGlobalModal: (params: OpenModalProps) => void;
  closeGlobalModal: () => void;
};

const useGlobalModal = create<GlobalModalStore>(set => ({
  state: {
    content: null,
    additionalActions: null,
    title: '',
    titleColor: theme.palette.primary.main,
    primaryAction: undefined,
    primaryActionText: undefined,
    closeAction: undefined,
    dialogProps: {
      open: false, // isOpen moved inside dialogProps
      fullWidth: true,
      maxWidth: 'sm', // Default modal size
    },
    formName: undefined,
    isDirtyCheck: false,
  },

  openGlobalModal: ({
    content,
    title,
    titleColor,
    primaryAction,
    primaryActionText,
    closeAction,
    dialogProps = {},
    formName,
    additionalActions,
    isDirtyCheck,
  }) =>
    set(state => ({
      state: {
        ...state.state,
        content,
        title,
        titleColor: titleColor ?? theme.palette.primary.main,
        primaryAction,
        primaryActionText,
        closeAction,
        dialogProps: { ...state.state.dialogProps, ...dialogProps, open: true }, // Ensure open is true
        formName,
        additionalActions,
        isDirtyCheck: isDirtyCheck ?? false,
      },
    })),

  closeGlobalModal: () =>
    set(state => ({
      state: {
        ...state.state,
        content: null,
        title: '',
        primaryAction: undefined,
        primaryActionText: undefined,
        closeAction: undefined,
        dialogProps: { ...state.state.dialogProps, open: false }, // Ensure open is false
        formName: undefined,
        additionalActions: null,
        isDirtyCheck: false,
      },
    })),
}));

export default useGlobalModal;
