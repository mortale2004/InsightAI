import { type ReactNode, useEffect, useRef } from "react"
import { cn } from "@repo/ui/lib/utils"


interface ConversationProps {
  children: ReactNode
  className?: string
}

export function Conversation({ children, className }: ConversationProps) {
  return <div className={cn("flex flex-col h-full", className)}>{children}</div>
}

interface ConversationContentProps {
  children: ReactNode
  className?: string
  autoScroll?: boolean
}

export function ConversationContent({ children, className, autoScroll = true }: ConversationContentProps) {
  const bottomRef = useRef<HTMLDivElement>(null)

  useEffect(() => {
    if (autoScroll) {
      bottomRef.current?.scrollIntoView({ behavior: "smooth" })
    }
  }, [children, autoScroll])

  return (
    <div className={cn("flex-1 overflow-y-auto px-6 py-6", className)}>
      <div className="space-y-6 max-w-4xl mx-auto">
        {children}
        <div ref={bottomRef} />
      </div>
    </div>
  )
}
