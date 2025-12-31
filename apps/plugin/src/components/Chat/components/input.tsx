import React, { RefObject, useState } from "react";
import { PromptInput } from "./prompt-input";
import { Button } from "@repo/ui/components/button";
import { SendHorizontal } from "lucide-react";
const Input: React.FC<{
  inputRef: RefObject<HTMLTextAreaElement | null>;
  sendMessage: () => void;
  isLoading: boolean;
  input: string;
  setInput: (input: string) => void;
}> = ({ inputRef, sendMessage, isLoading, input, setInput }) => {
  return (
    <div className="border-t border-border bg-background/80 backdrop-blur px-6 py-4 shrink-0">
      <div className="max-w-4xl mx-auto">
        <div className="flex items-end gap-3 bg-muted rounded-2xl px-4 py-3">
          <PromptInput
            ref={inputRef}
            value={input}
            onChange={(e) => setInput(e.target.value)}
            onSubmit={sendMessage}
            placeholder="Message InsightAI…"
          />
          <Button
            onClick={sendMessage}
            disabled={isLoading || !input.trim()}
            size="sm"
            className="shrink-0"
          >
            <SendHorizontal className="size-4" />
          </Button>
        </div>
        <p className="text-xs text-muted-foreground text-center mt-2">
          Press Enter to send • Shift + Enter for new line
        </p>
      </div>
    </div>
  );
};

export default Input;
