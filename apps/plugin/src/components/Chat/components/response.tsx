import { cn } from "@repo/ui/lib/utils"
import type { ReactNode } from "react"
import ReactMarkdown, { type Components } from "react-markdown"
import remarkGfm from "remark-gfm"

interface ResponseProps {
  children: string | ReactNode
  className?: string
}

const markdownComponents: Partial<Components> = {
  h1: ({ children }) => <h1 className="text-3xl font-bold mt-6 mb-4 text-balance">{children}</h1>,
  h2: ({ children }) => <h2 className="text-2xl font-bold mt-5 mb-3 text-balance">{children}</h2>,
  h3: ({ children }) => <h3 className="text-xl font-bold mt-4 mb-2 text-balance">{children}</h3>,
  h4: ({ children }) => <h4 className="text-lg font-semibold mt-3 mb-2">{children}</h4>,
  p: ({ children }) => <p className="leading-relaxed mb-4 last:mb-0 text-pretty">{children}</p>,
  ul: ({ children }) => <ul className="list-disc list-inside mb-4 space-y-1">{children}</ul>,
  ol: ({ children }) => <ol className="list-decimal list-inside mb-4 space-y-1">{children}</ol>,
  li: ({ children }) => <li className="leading-relaxed">{children}</li>,
  blockquote: ({ children }) => (
    <blockquote className="border-l-4 border-muted-foreground/20 pl-4 italic my-4 text-muted-foreground">
      {children}
    </blockquote>
  ),
  code: ({ className, children }) => {
    const isInline = !className
    if (isInline) {
      return <code className="bg-muted px-1.5 py-0.5 rounded text-sm font-mono">{children}</code>
    }
    return (
      <code className={cn("block bg-muted p-4 rounded-lg overflow-x-auto font-mono text-sm my-4", className)}>
        {children}
      </code>
    )
  },
  pre: ({ children }) => <pre className="my-4">{children}</pre>,
  a: ({ href, children }) => (
    <a
      href={href}
      target="_blank"
      rel="noopener noreferrer"
      className="text-primary underline hover:text-primary/80 transition-colors"
    >
      {children}
    </a>
  ),
  table: ({ children }) => (
    <div className="overflow-x-auto my-4">
      <table className="w-full border-collapse border border-border">{children}</table>
    </div>
  ),
  thead: ({ children }) => <thead className="bg-muted">{children}</thead>,
  tbody: ({ children }) => <tbody>{children}</tbody>,
  tr: ({ children }) => <tr className="border-b border-border">{children}</tr>,
  th: ({ children }) => <th className="px-4 py-2 text-left font-semibold">{children}</th>,
  td: ({ children }) => <td className="px-4 py-2">{children}</td>,
  hr: () => <hr className="my-6 border-border" />,
  strong: ({ children }) => <strong className="font-semibold">{children}</strong>,
  em: ({ children }) => <em className="italic">{children}</em>,
}

export function Response({ children, className }: ResponseProps) {
  if (typeof children === "string") {
    return (
      <div
        className={cn(
          "prose prose-sm max-w-none dark:prose-invert",
          "prose-headings:text-foreground prose-p:text-foreground",
          "prose-strong:text-foreground prose-code:text-foreground",
          "prose-li:text-foreground prose-blockquote:text-muted-foreground",
          className,
        )}
      >
        <ReactMarkdown remarkPlugins={[remarkGfm]} components={markdownComponents}>
          {children}
        </ReactMarkdown>
      </div>
    )
  }

  return <div className={cn("text-sm leading-relaxed", className)}>{children}</div>
}
