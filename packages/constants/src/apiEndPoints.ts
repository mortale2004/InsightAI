export type routeMethod = "Create" | "Update" | "Delete" | "Get" | "GetList";

export type RoutesType = {
  [key: string]: string | Partial<{ [key in routeMethod]: string }>;
};

export const routeMethods: {
  [key in routeMethod]: routeMethod;
} = {
  Create: "Create",
  Update: "Update",
  Delete: "Delete",
  Get: "Get",
  GetList: "GetList",
};

export const routeMethodsArray: routeMethod[] = Object.values(routeMethods);

export const adminApiEndPoints: RoutesType = {
  prompt: "/prompts",
  fileType: "/filetypes",
  user: "/users",
  register: {
    Create: "/register",
  },
};

export const pluginApiEndPoints: RoutesType = {
  execute: "/execute",
};
