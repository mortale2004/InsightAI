import { apiHooks } from "@/hooks/apiHooks";
import { useState, useRef, useEffect } from "react";

export default function Chat() {
  const [messages, setMessages] = useState([
    { role: "assistant", content: "Hello! How can I help you today?" },
  ]);
  const [input, setInput] = useState("");
  const bottomRef = useRef<HTMLDivElement | null>(null);

  const sendMessageApi = apiHooks?.execute?.useCreate({});

  const isResponsePending = sendMessageApi?.status === "pending";

  useEffect(() => {
    bottomRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [messages]);

  const sendMessage = () => {
    if (!input.trim()) return;
    setMessages((prev) => [...prev, { role: "user", content: input }]);
    setInput("");
    sendMessageApi.mutateAsync(
      {
        applicationName: "Xelence 7.0",
        fileType: "Form",
        fileName: "wfmLogin",
        regionName: "DEVELOPMENT",
        prompt: input,
        fileContent: `<!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>Login Page</title>
            <link rel="stylesheet" href="style.css">
        </head>
        <body>
            <div class="login-container">
                <form class="login-form" id="loginForm">
                    <h2>Login</h2>
                    <div class="input-group">
                        <label for="username">Username</label>
                        <input type="text" id="username" name="username" required>
                    </div>
                    <div class="input-group">
                        <label for="password">Password</label>
                        <input type="password" id="password" name="password" required>
                    </div>
                    <button type="submit">Log In</button>
                    <p id="message"></p>
                </form>
            </div>
            <script src="script.js"></script>
        </body>
        </html>
        `,
      },
      {
        onSuccess: (response: any) => {
          setMessages((prev) => [...prev, { role: "assistant", content: response }]);
        },
      }
    );
  };

  return (
    <div className="flex flex-col h-screen bg-[rgb(var(--bg-main))] text-[rgb(var(--text-main))]">
      {/* Header */}
      <div className="h-14 flex items-center px-6 border-b border-[rgb(var(--border-soft))] backdrop-blur bg-[rgb(var(--bg-main)/0.8)]">
        <span className="text-lg font-semibold">InsightAI</span>
      </div>

      {/* Messages */}
      <div className="flex-1 overflow-y-auto px-6 py-6 space-y-6">
        {messages.map((msg, i) => (
          <div key={i} className={`flex ${msg.role === "user" ? "justify-end" : "justify-start"}`}>
            <div
              className={`max-w-[75%] rounded-2xl px-4 py-3 text-sm leading-relaxed shadow-sm
                ${
                  msg.role === "user"
                    ? "bg-[rgb(var(--bg-user))] text-white rounded-br-sm"
                    : "bg-[rgb(var(--bg-assistant))] text-[rgb(var(--text-main))] rounded-bl-sm"
                }`}
            >
              {msg.content}
            </div>
          </div>
        ))}
        <div ref={bottomRef} />
      </div>

      {/* Input */}
      <div className="border-t border-[rgb(var(--border-soft))] bg-[rgb(var(--bg-main)/0.8)] backdrop-blur px-6 py-4">
        <div className="flex items-center gap-3 bg-[rgb(var(--bg-panel))] rounded-2xl px-4 py-3">
          <input
            value={input}
            onChange={(e) => setInput(e.target.value)}
            onKeyDown={(e) => e.key === "Enter" && sendMessage()}
            placeholder="Message InsightAI…"
            className="flex-1 bg-transparent outline-none text-sm placeholder:text-[rgb(var(--text-muted))]"
          />
          <button
            onClick={sendMessage}
            disabled={isResponsePending}
            className="bg-[rgb(var(--bg-user))] hover:opacity-90 transition px-4 py-2 rounded-xl text-sm font-medium text-white"
          >
            Send
          </button>
        </div>
      </div>
    </div>
  );
}
