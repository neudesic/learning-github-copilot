import axios from 'axios';
import { BASE_URL } from 'config/constants';

// Create a pre-configured Axios instance
const axiosClient = axios.create({
	baseURL: BASE_URL, // Set your API base URL here
	timeout: 10000,
	headers: {
		'Content-Type': 'application/json',
	},
});

export default axiosClient;
