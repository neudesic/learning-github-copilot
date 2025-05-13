import react from '@vitejs/plugin-react';
import path from 'path';
import { defineConfig } from 'vitest/config';

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  test: {
    environment: 'jsdom',
    globals: true, // Enables globals like `describe`, `it`, and `expect`
  },
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'),
      assets: path.resolve(__dirname, './src/assets'),
      components: path.resolve(__dirname, './src/components'),
      config: path.resolve(__dirname, './src/config'),
      constants: path.resolve(__dirname, './src/constants'),
      forms: path.resolve(__dirname, './src/forms'),
      helpers: path.resolve(__dirname, './src/helpers'),
      pages: path.resolve(__dirname, './src/pages'),
      service: path.resolve(__dirname, './src/service'),
      store: path.resolve(__dirname, './src/store'),
      types: path.resolve(__dirname, './src/types'),
    },
  },
});
