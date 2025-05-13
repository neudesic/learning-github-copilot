import { RouteObject } from 'react-router-dom';
import { PageContainer } from 'components/layouts/PageContainer';


const myRoutes: RouteObject[] = [
  {
    path: '/',
    element: <PageContainer />,
    children: [
      {
        path: '/',
        lazy: async () => {
          const Component = await import('../pages/Home')
          return { Component: Component.default };
        },
      },
      {
        path: '/company/:id',
        lazy: async () => {
          const Component = await import('../pages/CompanyPage');
          return { Component: Component.default };
        },
      },
    ],
  },
  {
    path: '*',
    element: <PageContainer />,
    children: [
      {
        path: '*',
        lazy: async () => {
          const Component = await import('../pages/NoContentPage');
          return { Component: Component.default };
        },
      },
    ],
  },
];

export { myRoutes };
