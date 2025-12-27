import { appPromptType } from "@/types/types";
import { atom } from "jotai";

export const sidebarAtom = atom(true);

export const appPromptAtom = atom<appPromptType | undefined>();
