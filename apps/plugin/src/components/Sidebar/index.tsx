import { sidebarAtom } from "@/store/app";
import { useAtomValue } from "jotai";
import { BadgePlus, Search } from "lucide-react";
import Header from "./Header";
import SidebarButton from "./SidebarButton";

const Sidebar = () => {
  const isOpen = useAtomValue(sidebarAtom);

  return (
    <section
      className={`${isOpen ? "w-[250px]" : "w-[55px]"} h-full  p-3
  transition-all duration-300 ease-in-out flex flex-col gap-3 border-r`}
    >
      <Header />

      <hr className="divider rounded-2xl" />

      <SidebarButton
        onClick={() => {
          alert("Create Chat");
        }}
        Icon={BadgePlus}
        text="Create"
      />

      <SidebarButton
        onClick={() => {
          alert("Search Chat");
        }}
        Icon={Search}
        text="Search"
      />
    </section>
  );
};

export default Sidebar;
