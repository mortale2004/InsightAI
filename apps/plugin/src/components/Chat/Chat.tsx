import { apiHooks } from "@/hooks/apiHooks";
import { appPromptAtom } from "@/store/app";
import { useAtom } from "jotai";
import { useEffect, useRef, useState } from "react";
import { Conversation } from "./components/conversasion";
import Header from "./components/Header";
import Input from "./components/input";
import Messages from "./components/Messages";
import { Message } from "./types";
const Chat = () => {
  const [messages, setMessages] = useState<Message[]>([
    { role: "assistant" as const, content: "Hello! How can I help you today?" },
  ]);
  
  const inputRef = useRef<HTMLTextAreaElement>(null);
  const [appPrompt, setAppPrompt] = useAtom(appPromptAtom);
  const bottomRef = useRef<HTMLDivElement | null>(null);
  const [input, setInput] = useState("");
  
  // const [isLoading, setIsLoading] = useState(false);
  const sendMessageApi = apiHooks?.execute?.useCreate({});
  const isLoading = sendMessageApi?.status === "pending";

   const sendMessage = () => {
    if (!input.trim()) return;
    setMessages((prev) => [...prev, { role: "user", content: input }]);
    // setIsLoading(true);
//     setTimeout(()=>{
//       setMessages((prev)=>([
//         ...prev,
//         {
//           role:"assistant",
// content: responses[Math.floor(Math.random() * responses.length)]
//         }
//       ]))
//       setIsLoading(false);
//     }, 1000)
    sendMessageApi.mutateAsync(
      {
        ...appPrompt,
        prompt: input,
      },
      {
        onSuccess: (response: any) => {
          if (!appPrompt?.userChatId){
            setAppPrompt(appPrompt ? {
              ...appPrompt,
              userChatId: response?.userChatId,
            } : undefined)
          }
          setMessages((prev) => [...prev, { role: "assistant", content: response?.responseText }]);
        },
      }
    );
    setInput("");
  };


  useEffect(() => {
    bottomRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [messages]);

  // Auto-resize textarea
  useEffect(() => {
    if (inputRef.current) {
      inputRef.current.style.height = "auto";
      inputRef.current.style.height = `${inputRef.current.scrollHeight}px`;
    }
  }, [input]);


  return (
    <Conversation className="h-screen bg-background text-foreground">
      <Header />
      <Messages messages={messages} isLoading={isLoading} />
      <Input
        inputRef={inputRef}
        isLoading={isLoading}
        sendMessage={sendMessage}
        input={input}
        setInput={setInput}
      />
    </Conversation>
  );
};
export default Chat;


// const responses = [
//   `# ChatGPT-Style Assistant Response (Markdown)

// This is an example of a **large AI response** rendered using Markdown.  
// It is designed to test **full-width layout**, spacing, and readability.

// ---

// ## Why Assistant Messages Are Full Width

// Assistant messages often contain:
// - Long explanations
// - Code blocks
// - Lists
// - Tables
// - Quotes

// Confining them inside chat bubbles reduces readability.

// > **Design principle:**  
// > If the message explains or teaches, let it breathe.

// ---

// ## Feature Breakdown

// ### User Messages
// - Right aligned
// - Light grey background
// - Short and contextual

// ### Assistant Messages
// - Left aligned
// - No background
// - Full width
// - Optimized for reading

// ---

// ## Example Code Block

// \`\`\`tsx
// export function Message({ from, children }: MessageProps) {
//   const isUser = from === "user"

//   return (
//     <div className={isUser ? "justify-end" : "justify-start"}>
//       <div className={isUser ? "max-w-[70%]" : "w-full"}>
//         {children}
//       </div>
//     </div>
//   )
// }
// \`\`\`
// `,
// `
// If you want:
// - **Even larger markdown**
// - **Tables**
// - **Error-style AI messages**
// - **Streaming partial markdown**

// Just tell me 👍
// `,
// `# Building a ChatGPT-Like Chat Interface

// This response is intentionally **long and structured** to validate how your UI handles
// full-width assistant messages, spacing, and markdown rendering.

// ---

// ## Core UI Philosophy

// ChatGPT does **not** treat assistant responses as chat bubbles.

// Why?

// - Assistant responses are **instructional**
// - Messages often contain **code, lists, and explanations**
// - Constraining width harms readability

// > **Key idea:**  
// > The assistant is not chatting — it is *answering*.

// ---

// ## Visual Differences Between Roles

// ### User
// - Compact layout
// - Light grey background
// - Right aligned
// - Conversational tone

// ### Assistant
// - Full width
// - No background
// - Left aligned
// - Documentation-style tone

// This distinction improves scanning and reduces cognitive load.

// ---

// ## Long Code Example (Markdown Stress Test)

// \`\`\`tsx
// import { cn } from "@/lib/utils"

// interface MessageProps {
//   from: "user" | "assistant"
//   children: React.ReactNode
// }

// export function Message({ from, children }: MessageProps) {
//   const isUser = from === "user"

//   return (
//     <div className={cn("w-full flex", isUser ? "justify-end" : "justify-start")}>
//       <div
//         className={cn(
//           "flex gap-3",
//           isUser ? "max-w-[70%] flex-row-reverse" : "w-full"
//         )}
//       >
//         {children}
//       </div>
//     </div>
//   )
// }
// `
// ]