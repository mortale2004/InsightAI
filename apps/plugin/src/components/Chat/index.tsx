import { useState, useRef, useEffect } from "react";


export default function Chat() {
  const [messages, setMessages] = useState([
    { role: "assistant", content: "Hello! How can I help you today?" },
  ]);
  const [input, setInput] = useState("");
  const bottomRef = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    bottomRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [messages]);

  const sendMessage = () => {
    if (!input.trim()) return;

    setMessages((prev) => [...prev, { role: "user", content: input }]);
    setInput("");

    setTimeout(() => {
      setMessages((prev) => [
        ...prev,
        { role: "assistant", content: "This is a InsightAI-style response." },
      ]);
    }, 900);
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
          <div
            key={i}
            className={`flex ${msg.role === "user" ? "justify-end" : "justify-start"}`}
          >
            <div
              className={`max-w-[75%] rounded-2xl px-4 py-3 text-sm leading-relaxed shadow-sm
                ${msg.role === "user"
                  ? "bg-[rgb(var(--bg-user))] text-white rounded-br-sm"
                  : "bg-[rgb(var(--bg-assistant))] text-[rgb(var(--text-main))] rounded-bl-sm"}`}
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
            className="bg-[rgb(var(--bg-user))] hover:opacity-90 transition px-4 py-2 rounded-xl text-sm font-medium text-white"
          >
            Send
          </button>
        </div>
      </div>
    </div>
  );
}
