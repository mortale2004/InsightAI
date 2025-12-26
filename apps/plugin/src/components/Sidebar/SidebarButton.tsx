import { sidebarAtom } from "@/store/app";
import React from "react";
import { useAtomValue } from "jotai";

const SidebarButton: React.FC<{
  Icon: React.FC<any>;
  text: string;
  onClick: React.MouseEventHandler<HTMLButtonElement> | undefined;
}> = ({ Icon, text, onClick }) => {
  const isOpen = useAtomValue(sidebarAtom);
  return (
    <button onClick={onClick} className="flex gap-2 items-center cursor-pointer">
      <Icon />
      {isOpen && text}
    </button>
  );
};

export default SidebarButton;
