import React from "react";

const Header: React.FC = () => {
  return (
    <div className="h-14 flex items-center px-6 border-b border-border backdrop-blur bg-background/80 shrink-0">
      <span className="text-lg font-semibold">InsightAI</span>
    </div>
  );
};

export default Header;
