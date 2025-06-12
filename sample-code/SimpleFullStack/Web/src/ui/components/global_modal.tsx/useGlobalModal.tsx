import { create } from 'zustand';
import type { DialogProps } from '@mui/material/Dialog';
import type { ReactNode } from 'react';

export type GlobalModalStore = {
    isOpen: boolean;
    content: ReactNode | null;
    dialogProps?: Omit<DialogProps, 'open' | 'onClose' | 'children'>;
    title?: ReactNode;
    titleColor?: string;
    primaryAction?: () => void;
    primaryActionText?: string;
    closeAction?: () => void;
    formName?: string;
    open: (
        content: ReactNode,
        options?: {
            dialogProps?: GlobalModalStore['dialogProps'];
            title?: ReactNode;
            titleColor?: string;
            primaryAction?: () => void;
            primaryActionText?: string;
            closeAction?: () => void;
            formName?: string;
        }
    ) => void;
    close: () => void;
};

const useGlobalModal = create<GlobalModalStore>(set => ({
    isOpen: false,
    content: null,
    dialogProps: undefined,
    title: undefined,
    titleColor: undefined,
    primaryAction: undefined,
    primaryActionText: undefined,
    closeAction: undefined,
    formName: undefined,
    open: (content, options) => set({
        isOpen: true,
        content,
        dialogProps: options?.dialogProps,
        title: options?.title,
        titleColor: options?.titleColor,
        primaryAction: options?.primaryAction,
        primaryActionText: options?.primaryActionText,
        closeAction: options?.closeAction,
        formName: options?.formName,
    }),
    close: () => set({
        isOpen: false,
        content: null,
        dialogProps: undefined,
        title: undefined,
        titleColor: undefined,
        primaryAction: undefined,
        primaryActionText: undefined,
        closeAction: undefined,
        formName: undefined,
    }),
}));

export default useGlobalModal;
