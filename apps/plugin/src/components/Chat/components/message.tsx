import { cn } from "@repo/ui/lib/utils"
import type { ReactNode } from "react"

interface MessageProps {
  from: "user" | "assistant"
  children: ReactNode
  className?: string
}

export function Message({ from, children, className }: MessageProps) {
  return (
    <div
      className={cn(
        "flex gap-4 w-full",
        from === "user" ? "justify-end" : "justify-start",
        className
      )}
    >
      <div
        className={cn(
          "flex gap-3",
          from === "user"
            ? "flex-row-reverse max-w-[70%]"
            : "flex-row w-full"
        )}
      >
        {/* Content */}
        <div
          className={cn(
            "flex flex-col gap-2 min-w-0",
            from === "user"
              ? "bg-muted rounded-xl px-4 py-2"
              : "w-full"
          )}
        >
          {children}
        </div>
      </div>
    </div>
  )
}

interface MessageContentProps {
  children: ReactNode
  className?: string
}

export function MessageContent({ children, className }: MessageContentProps) {
  return (
    <div className={cn("flex flex-col gap-2", className)}>
      {children}
    </div>
  )
}
