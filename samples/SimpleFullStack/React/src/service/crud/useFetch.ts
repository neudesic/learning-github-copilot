import { useQuery } from '@tanstack/react-query';
import { apiClient } from '../axiosConfig';
import { DEFAULT_PAGING_OPTIONS, FetchParams } from 'types/helperTypes';

const useFetch = <TReturn>({
  url,
  config,
  queryKey,
  search,
  pagingOptions = DEFAULT_PAGING_OPTIONS,
  shouldFetch = true,
  refetchOnWindowFocus = false,
  refetchOnReconnect = true,
  mockData,
}: FetchParams<TReturn>) => {
  // Modify queryKey based on search and pagingOptions
  const finalQueryKey = [...queryKey];

  if (search) {
    finalQueryKey.push(search);
  }

  if (pagingOptions) {
    if (pagingOptions.page) {
      finalQueryKey.push(pagingOptions.page.toString());
    }
    if (pagingOptions.pageSize) {
      finalQueryKey.push(pagingOptions.pageSize.toString());
    }
  }

  // Fetch
  const { isLoading, isError, data } = useQuery({
    queryKey: finalQueryKey, // Use the updated queryKey
    queryFn: async () => {
      if (!shouldFetch) {
        return null;
      }
      // Build the params object dynamically
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      const params: Record<string, any> = {};

      if (search) {
        params.search = search;
      }

      if (pagingOptions) {
        params.page = pagingOptions.page;
        params.pageSize = pagingOptions.pageSize;
      }

      if (config?.params) {
        config.params = {
          ...config.params,
          ...params,
        };
      }

      //mocking data
      if (mockData) {
        return mockData;
      }

      // Make the API request with the built params object
      const response = await apiClient.get(url, {
        params,
        ...config,
      });

      return response.data as TReturn;
    },
    enabled: shouldFetch,
    refetchOnWindowFocus,
    refetchOnReconnect,
  });

  return { isLoading, isError, data };
};

export default useFetch;
