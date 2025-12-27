import React from "react";

type AssistantProps = {
  content: any;
  type: any;
};
const Assistant: React.FC<AssistantProps> = ({ content }) => {
  return (
    <div className={`flex justify-start`}>
      <div
        className={`max-w-[75%] rounded-2xl px-4 py-3 text-sm leading-relaxed shadow-sm bg-[rgb(var(--bg-assistant))] text-[rgb(var(--text-main))] rounded-bl-sm`}
        dangerouslySetInnerHTML={{ __html: content }}
      ></div>
    </div>
  );
};

export default Assistant;
