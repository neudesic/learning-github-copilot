import { create } from 'zustand';

type OpenSidePanelParams = {
    content: JSX.Element;
    title: string;
}

type SidePanelStore = {
    isOpen: boolean;
    content: JSX.Element | null;
    title: string;
    open: (params: OpenSidePanelParams) => void;
    close: () => void;
};

const useGlobalSidePanel = create<SidePanelStore>(set => ({
    isOpen: false,
    content: null,
    title: '',
    open: ({ content, title }) => set({ isOpen: true, content, title}),
    close: () => set({ isOpen: false, content: null }),
}));

export default useGlobalSidePanel;