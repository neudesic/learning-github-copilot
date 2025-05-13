import React from 'react';
import { create } from 'zustand';
import { DialogProps } from '@mui/material/Dialog';
import { theme } from '../../styles/themes';
import { CustomModalProps, OpenModalProps } from './useGlobalModal';

type GlobalSecodaryModalStore = {
  globalSecondaryState: {
    content: React.ReactNode | null;
    dialogProps: DialogProps; // Now holds isOpen and all Dialog properties
  } & CustomModalProps;
  openSecondaryModal: (params: OpenModalProps) => void;
  closeSecondaryModal: () => void;
};

const useGlobalModal = create<GlobalSecodaryModalStore>(set => ({
  globalSecondaryState: {
    content: null,
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
    isDirty: false,
  },

  openSecondaryModal: ({
    content,
    title,
    titleColor,
    primaryAction,
    primaryActionText,
    closeAction,
    dialogProps = {},
    formName,
  }) =>
    set(state => ({
      globalSecondaryState: {
        ...state.globalSecondaryState,
        content,
        title,
        titleColor: titleColor ?? theme.palette.primary.main,
        primaryAction,
        primaryActionText,
        closeAction,
        dialogProps: {
          ...state.globalSecondaryState.dialogProps,
          ...dialogProps,
          open: true,
        }, // Ensure open is true
        formName,
      },
    })),

  closeSecondaryModal: () =>
    set(state => ({
      globalSecondaryState: {
        ...state.globalSecondaryState,
        content: null,
        title: '',
        primaryAction: undefined,
        primaryActionText: undefined,
        closeAction: undefined,
        dialogProps: {
          ...state.globalSecondaryState.dialogProps,
          open: false,
        }, // Ensure open is false
        formName: undefined,
      },
    })),
}));

export default useGlobalModal;
