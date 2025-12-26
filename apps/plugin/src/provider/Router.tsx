import { BrowserRouter, Route, Routes } from "react-router-dom";
import Landing from "@/pages/landing";

const Router = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" Component={Landing} />
      </Routes>
    </BrowserRouter>
  );
};

export default Router;
