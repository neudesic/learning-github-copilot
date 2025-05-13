import { CustomPaletteOptions } from '../styles/themes'; // Adjust the import path according to your project structure

declare module '@mui/material/styles' {
  interface Palette {
    custom: CustomPaletteOptions;
  }

  interface PaletteOptions {
    custom?: CustomPaletteOptions;
  }
}
