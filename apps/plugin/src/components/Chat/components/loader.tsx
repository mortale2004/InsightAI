import { cn } from "@repo/ui/lib/utils";
import { Spinner } from "@repo/ui/components/spinner";
interface LoaderProps {
  className?: string;
}

export function Loader({ className }: LoaderProps) {
  return (
    <div
      className={cn("flex items-center gap-2 text-muted-foreground", className)}
    >
      <Spinner className="size-4" />
      <span className="text-sm">Thinking...</span>
    </div>
  );
}
