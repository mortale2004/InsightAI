"use client"

import type React from "react"

import { cn } from "@repo/ui/lib/utils"
import { forwardRef, type KeyboardEvent } from "react"

interface PromptInputProps extends React.TextareaHTMLAttributes<HTMLTextAreaElement> {
  onSubmit?: () => void
}

export const PromptInput = forwardRef<HTMLTextAreaElement, PromptInputProps>(
  ({ className, onSubmit, onKeyDown, ...props }, ref) => {
    const handleKeyDown = (e: KeyboardEvent<HTMLTextAreaElement>) => {
      if (e.key === "Enter" && !e.shiftKey) {
        e.preventDefault()
        onSubmit?.()
      }
      onKeyDown?.(e)
    }

    return (
      <textarea
        ref={ref}
        rows={1}
        className={cn(
          "flex-1 bg-transparent outline-none text-sm resize-none",
          "placeholder:text-muted-foreground",
          "max-h-32 min-h-6",
          className,
        )}
        onKeyDown={handleKeyDown}
        {...props}
      />
    )
  },
)

PromptInput.displayName = "PromptInput"
