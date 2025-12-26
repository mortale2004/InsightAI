import React from "react";
import QueryProvider from "./QueryProvider";
import ToastProvider from "./ToastProvider";
import Router from "./Router";

const Provider: React.FC<{}> = () => {
  return (
    <QueryProvider>
      <Router />
      <ToastProvider />
    </QueryProvider>
  );
};

export default Provider;
