import axios from 'axios';

export const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || '',
  headers: {
    'Content-Type': 'application/json',
    Accept: 'application/json',
  },
});

//middle ware for setting headers.
apiClient.interceptors.request.use(config => {
  return config;
});
