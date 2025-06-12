import type { RouteObject } from 'react-router-dom';
import MainLayout from 'ui/components/layouts/MainLayout';
import { APPLICATION } from './constants';
import NotFoundPage from 'ui/components/boundaries/NotFoundPage';

const myRoutes: RouteObject[] = [
    {
        path: APPLICATION.Home.route,
        element: <MainLayout />,
        children: [{
            path: APPLICATION.Home.route,
            lazy: async () => {
                const Component = await import('ui/pages/home/HomePage');
                return { Component: Component.default };
            },
        }, {
            path: APPLICATION.Products.route,
            lazy: async () => {
                const Component = await import('ui/pages/product/ProductsPage');
                return { Component: Component.default };
            },
        },
        {
            path: `${APPLICATION.Products.route}/:productId`,
            lazy: async () => {
                const Component = await import('ui/pages/product/ProductDetailPage');
                return { Component: Component.default };
            },
        },
        {
            path: '*',
            element: <NotFoundPage />,
        },
        ],
    },
];

export default myRoutes;
