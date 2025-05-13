import { AxiosRequestConfig } from 'axios';

export type Prettify<T> = { [K in keyof T]: T[K] } & object;

export type OptionType = {
  id: string;
  name?: string;
  label?: string;
  value: string;
};

export type FetchParams<TReturn> = {
  url: string;
  config?: AxiosRequestConfig | undefined;
  queryKey: string[];
  search?: string;
  pagingOptions?: {
    page: number;
    pageSize: number;
  };
  shouldFetch?: boolean;
  refetchOnWindowFocus?: boolean;
  refetchOnReconnect?: boolean;
  mockData?: TReturn;
};

export type PagingOptions = {
  page: number;
  pageSize: number;
};

export const DEFAULT_PAGING_OPTIONS: PagingOptions = {
  page: 1,
  pageSize: 25,
};
