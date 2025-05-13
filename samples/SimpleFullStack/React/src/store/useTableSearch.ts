import { create } from "zustand";

type TableSearch = {
  search: string;
  setSearch: (search: string) => void;
  pagingOptions: {
    page: number;
    pageSize: number;
  };
  setPagingOptions: (pagingOptions: { page: number; pageSize: number }) => void;
  setPagingOptiongDefault: () => void;
};

export const useTableSearch = create<TableSearch>((set) => ({
  search: "",
  setSearch: (search) => set({ search }),
  pagingOptions: {
    page: 1,
    pageSize: 10,
  },
  setPagingOptions: (pagingOptions) => set({ pagingOptions }),
  setPagingOptiongDefault: () =>
    set({ pagingOptions: { page: 1, pageSize: 10 } }),
}));
