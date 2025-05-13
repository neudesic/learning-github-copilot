import { useMutation, useQueryClient } from '@tanstack/react-query';
import useSnackbar from '../../store/global/useGlobalSnackbar';
import { apiClient } from '../axiosConfig';

const useDelete = ({
  url,
  queryKey,
  snackMessage = 'Deleted successfully',
  onSuccess,
  onError,
}: {
  url: string;
  queryKey: string[];
  snackMessage?: string;
  onSuccess?: () => void;
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  onError?: (error: any) => void;
}) => {
  const queryClient = useQueryClient();
  const snackbar = useSnackbar();

  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  const params: Record<string, any> = {};

  const master_code = import.meta.env.VITE_AZURE_FUNCTION_MASTER_CODE || '';

  if (master_code) {
    params.code = master_code;
  }

  const deleteMutation = useMutation({
    mutationFn: async (id: string) => {
      const response = await apiClient.delete(url, {
        params: {
          ...params,
          id,
        },
      });

      return response.data;
    },

    onSuccess: () => {
      if (onSuccess) {
        onSuccess();
      }

      snackbar.open(snackMessage, 'success');

      //refetch categories
      queryClient.refetchQueries({
        queryKey,
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

  return { deleteMutation };
};
export default useDelete;
