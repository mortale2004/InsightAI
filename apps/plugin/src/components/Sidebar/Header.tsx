import { sidebarAtom } from "@/store/app";
import { useAtom } from "jotai";
import { Columns2 } from "lucide-react";
const Header = () => {
  const [isOpen, setIsOpen] = useAtom(sidebarAtom);

  return (
    <div className="flex justify-between">
      {isOpen && (
        <div>
          <img className="w-5 h-5" src={"/logo.png"} alt="Logo" />
        </div>
      )}
      <button
        onClick={() => {
          setIsOpen((prev) => !prev);
        }}
      >
        <Columns2 className="cursor-pointer" />
      </button>
    </div>
  );
};

export default Header;
