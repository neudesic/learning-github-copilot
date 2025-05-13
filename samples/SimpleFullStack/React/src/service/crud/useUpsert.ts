import { useMutation, useQueryClient } from '@tanstack/react-query';
import useSnackbar from '../../store/global/useGlobalSnackbar';
import { apiClient } from '../axiosConfig';

const useUpsert = <T extends { id: string }>({
  url,
  queryKey,
  onSuccess,
  onError,
  snackbarMessage,
}: {
  url: string;
  queryKey: string[];
  onSuccess?: (data: T) => void;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  onError?: (error: any) => void;
  snackbarMessage?: string;
}) => {
  const snackbar = useSnackbar();
  const queryClient = useQueryClient();

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  const params: Record<string, any> = {};

  const master_code = import.meta.env.VITE_AZURE_FUNCTION_MASTER_CODE || '';

  if (master_code) {
    params.code = master_code;
  }

  //Post
  const upsertMutation = useMutation({
    mutationFn: async (data: T) => {
      const response = await apiClient.put(url, data, {
        params,
      });
      return response.data as T;
    },
    onSuccess: (data: T) => {
      snackbar.open(snackbarMessage || 'Successfully added', 'success');

      if (onSuccess) {
        onSuccess(data);
      }

      //refetch categories
      queryClient.refetchQueries({
        queryKey,
      });

      //refetch item
      queryClient.refetchQueries({
        queryKey: [...queryKey, data.id],
      });
    },
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    onError: (error: any) => {
      snackbar.open(error.message, 'error');

      if (onError) {
        onError(error);
      }
    },
  });

  return { upsertMutation };
};
export default useUpsert;
