import { useAtom } from "jotai";
import { useEffect } from "react";
import Provider from "./provider";
import { appPromptAtom } from "./store/app";
import { appPromptType } from "./types/types";
import "./index.css";

export const App: React.FC<{
  appPrompt: appPromptType;
}> = ({ appPrompt }) => {
  const [hasAppPrompt, setAppPrompt] = useAtom(appPromptAtom);

  useEffect(() => {
    setAppPrompt(appPrompt);
  }, [setAppPrompt]);

  if (!hasAppPrompt) {
    return null;
  }

  return <Provider />;
};
