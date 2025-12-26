import { Bounce, toast as toastify, ToastOptions } from "react-toastify";

type ToastVariant = "success" | "error" | "warn";

const toastOptions: ToastOptions = {
  position: "top-center",
  autoClose: 3000,
  hideProgressBar: false,
  closeOnClick: true,
  pauseOnHover: true,
  draggable: true,
  progress: undefined,
  theme: "dark",
  transition: Bounce,
};

export const toast = (message: string, variant:ToastVariant = "success") => {
  toastify[variant](message, toastOptions);
};

