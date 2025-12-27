import path from "node:path";
import { defineConfig, loadEnv } from "vite";
import tailwindcss from "@tailwindcss/vite";

export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), "");
  const isWidgetBuild = mode === "widget";

  const rootEnv = loadEnv(mode, path.resolve(process.cwd(), "../../"), "VITE_");

  return {
    define: {
      "import.meta.env.VITE_APP_API_URL": JSON.stringify(rootEnv.VITE_APP_API_URL),
    },
    plugins: [tailwindcss()],

    resolve: {
      alias: {
        "@": path.resolve(__dirname, "./src"),
      },
    },

    // ---------- DEV SERVER ----------
    server: {
      port: Number.parseInt(env.PORT || "5173"),
    },

    // ---------- BUILD CONFIG ----------
    build: isWidgetBuild
      ? {
          lib: {
            entry: "src/app/widget.tsx",
            name: "InsightAI",
            formats: ["umd"],
            fileName: () => "insightai.min.js",
          },
          cssCodeSplit: false,
          rollupOptions: {
            external: [],
            output: {
              inlineDynamicImports: true,
            },
          },
        }
      : {
          // 👇 NORMAL APP BUILD (dev / preview)
          rollupOptions: {
            output: {
              manualChunks: {
                vendors: ["react", "react-dom"],
              },
            },
          },
        },
  };
});
