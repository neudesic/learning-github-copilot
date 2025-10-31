/// <reference types="node" />

import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import { resolve } from 'path';

export default defineConfig({
	base: '/',
	plugins: [react()],
	resolve: {
		alias: {
			// Core app directories
			ui: resolve(__dirname, 'src/ui'),
			store: resolve(__dirname, 'src/store'),
			config: resolve(__dirname, 'src/config'),
			services: resolve(__dirname, 'src/services'),
			assets: resolve(__dirname, 'src/assets'),
			json: resolve(__dirname, 'src/json'),
			types: resolve(__dirname, 'src/types'),

			// UI subdirectories for easier imports
			components: resolve(__dirname, 'src/ui/components'),
			pages: resolve(__dirname, 'src/ui/pages'),

			// Root src directory for absolute imports
			src: resolve(__dirname, 'src'),
		},
	},
});
