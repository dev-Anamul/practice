// src/global.d.ts

interface Window {
  RUNTIME_ENV?: {
    VITE_API_URL?: string;
    VITE_ANOTHER_VAR?: string;
    VITE_YET_ANOTHER_VAR?: string;
    // Add more variables as needed
  };
}
