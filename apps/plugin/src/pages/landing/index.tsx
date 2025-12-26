import Chat from "@/components/Chat";
import Sidebar from "@/components/Sidebar";

const Landing = () => {
  return (
    <main className="flex h-dvh">
      <Sidebar />
      <div className="w-full">
        <Chat />
      </div>
    </main>
  );
};

export default Landing;
