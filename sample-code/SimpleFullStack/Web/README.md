# Copilot Labs React Demo App

This is a demo React application created for exploring and showcasing GitHub Copilot Labs features. It is designed as a modern, responsive personal website and serves as a hands-on playground for Copilot Labs users to experiment with code generation, editing, and best practices in a real-world React project.

## Demo Purpose

- **Showcase Copilot Labs capabilities in a real React project**
- **Provide a safe environment for learning and experimenting with Copilot**
- **Demonstrate modern React, TypeScript, and Vite project structure**

## Tech Stack

- **React** (with Vite)
- **TypeScript**
- **Material UI (MUI)**
- **Zustand** (state management)
- **Axios** (HTTP requests)
- **React Hook Form** (form management)
- **React Router DOM** (routing)
- **React Query** (data fetching)

## Project Structure

```
Web/
├── public/                # Static assets
├── src/
│   ├── App.tsx            # Main app component
│   ├── main.tsx           # Entry point
│   ├── assets/            # Images and icons
│   ├── config/            # App configuration
│   │   └── json/          # JSON data for skills, projects, experience
│   ├── services/          # API and data services
│   ├── store/             # Zustand state management
│   └── ui/
│       ├── components/    # Reusable UI components
│       │   ├── icon_service/   # Centralized icon mapping and service
│       │   ├── infinite_icon_carousel/ # Animated skill carousel
│       │   ├── tech/      # TechChip and tech icon mapping
│       ├── forms/         # Form components
│       ├── nav/           # Navigation components
│       └── pages/         # Route pages (blog, home, projects, etc.)
├── package.json           # Project dependencies
├── tsconfig.json          # TypeScript configuration
├── vite.config.ts         # Vite configuration
└── README.md              # Project overview and setup (this file)
```

## Getting Started

1. **Install dependencies:**
   ```powershell
   npm install
   ```
2. **Start the development server:**
   ```powershell
   npm run dev
   ```

The app will be available at the local address shown in your terminal (typically http://localhost:5173).

---

## Data-Driven Features

- **Skills Carousel:** Powered by JSON for easy updates
- **Featured Projects:** JSON-driven, type-safe project data
- **Centralized Icon Service:** Consistent icon rendering across the app

## Planned Features

- Home, Projects, Blog, and Contact pages
- Responsive, accessible Material UI design
- State management with Zustand
- Async data with React Query
- Easy content updates via JSON

---

This project is intended for demo and educational purposes as part of Copilot Labs. Feel free to experiment, break things, and learn!
