import { App } from "@/App";
import { appPromptType } from "@/types/types";
import { createRoot, Root } from "react-dom/client";

/* ---------- INTERNAL ROOT REGISTRY ---------- */

const roots = new Map<string, Root>();

const getElementById = (elementId: string): HTMLElement => {
  const element = document.getElementById(elementId);
  if (!element) {
    throw new Error(`Element with id "${elementId}" not found in DOM`);
  }
  return element;
};

/* ---------- PUBLIC SDK ---------- */
const InsightAI = {
  mount: (elementId: string, appPrompt: appPromptType) => {
    const element = getElementById(elementId);

    let root = roots.get(elementId);

    // Create root only once
    if (!root) {
      root = createRoot(element);
      roots.set(elementId, root);
    }

    // React diffing will handle updates
    root.render(<App appPrompt={appPrompt} />);
  },

  unmount: (elementId: string) => {
    const root = roots.get(elementId);
    if (root) {
      root.unmount();
      roots.delete(elementId);
    }
  },
};

(window as any).InsightAI = InsightAI;

export default InsightAI;
