import PersonIcon from '@mui/icons-material/Person';
import ShoppingBasketIcon from '@mui/icons-material/ShoppingBasket';

export const APPLICATION = {
	Home: {
		id: 'home',
		label: 'Home',
		route: '/',
		icon: PersonIcon,
	},
	Products: {
		id: 'products',
		label: 'Products',
		route: '/products',
		icon: ShoppingBasketIcon,
	},
};

//Make Applicattion an array
export const APPLICATION_ARRAY = Object.values(APPLICATION);

export type ApplicationType = (typeof APPLICATION)[keyof typeof APPLICATION];

export const USER_NAME = 'Neudesic';
export const BASE_URL = import.meta.env.VITE_BASE_URL || '';
