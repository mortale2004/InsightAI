import React from "react";

type UserProps = {
  content: string;
};

const User: React.FC<UserProps> = ({ content }) => {
  return (
    <div className={`flex justify-end`}>
      <div
        className={`max-w-[75%] rounded-2xl px-4 py-3 text-sm leading-relaxed shadow-sm bg-[rgb(var(--bg-user))] text-white rounded-br-sm`}
      >
        {content}
      </div>
    </div>
  );
};

export default User;
