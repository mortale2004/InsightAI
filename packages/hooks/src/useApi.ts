import { toast } from "@repo/utils";
import {
  useMutation,
  UseMutationOptions,
  useQuery,
  UseQueryOptions,
  UseQueryResult,
} from "@tanstack/react-query";
import axios from "axios";
import {
  routeMethod,
  routeMethodsArray,
  RoutesType,
  routeMethods,
} from "@repo/constants";

const API_URL = // @ts-ignore
  import.meta.env.VITE_APP_API_URL;

// Create an Axios instance
export const api = axios.create({
  baseURL: API_URL,
  withCredentials: true,
});

api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;
    if (error.response?.status === 403 && !originalRequest._retry) {
      originalRequest._retry = true;
      try {
        // Attempt to refresh the token
        await axios.post(
          API_URL + "refreshtoken",
          {},
          { withCredentials: true },
        );
        return api(originalRequest); // Retry the original request
      } catch (err) {
        // @ts-ignore
        window.location.href = "/login"; // Redirect to login
      }
    }

    return Promise.reject(error);
  },
);

type AxiosMethods = "get" | "post" | "put" | "delete" | "patch";

export const fetch = async (method: AxiosMethods, url: string, data: any) => {
  try {
    if (method === "get") {
      return (await api.get(url, { params: data })).data;
    } else if (method === "post" || method === "put") {
      return (await api.post(url, { formData: data })).data;
    } else {
      return (await api.delete(`${url}/${data}`)).data;
    }
  } catch (error) {
    throw error;
  }
};

// Error handler function
export const onErrorHandler = (error: any) => {
  toast(error?.response?.data?.message || "Something went wrong!", "error");
};

// Success handler function
export const onSuccessHandler = (data: any) => {
  data.message && toast(data.message, "success");
};

// Create Data (POST)
export const useCreateData = <TVariables, TResponse>(
  endpoint: string,
  options?: UseMutationOptions<TResponse, any, TVariables>,
  onSuccess?: Function,
  onError?: Function,
  defaultHandlers: boolean = true,
) => {
  return useMutation<TResponse, any, TVariables>({
    mutationFn: async (payload: TVariables) => {
      const { data } = await api.post(endpoint, payload);
      return data;
    },
    onSuccess: (response) => {
      if (defaultHandlers) onSuccessHandler(response);
      onSuccess?.(response);
    },
    onError: (error) => {
      if (defaultHandlers) onErrorHandler(error);
      onError?.(error);
    },
    ...options,
  });
};

// Update Data (PUT)
export const useUpdateData = <TVariables, TResponse>(
  endpoint: string,
  options?: UseMutationOptions<TResponse, any, TVariables>,
  onSuccess?: Function,
  onError?: Function,
  defaultHandlers: boolean = true,
) => {
  return useMutation<TResponse, any, TVariables>({
    mutationFn: async (payload: TVariables) => {
      const { data } = await api.put(endpoint, payload);
      return data;
    },
    onSuccess: (response) => {
      if (defaultHandlers) onSuccessHandler(response);
      onSuccess?.(response);
    },
    onError: (error) => {
      if (defaultHandlers) onErrorHandler(error);
      onError?.(error);
    },
    ...options,
  });
};

// Read Data (GET)
export const useGetData = <TResponse>(
  endpoint: string,
  params?: any,
  options: UseQueryOptions<TResponse, any> = {
    queryKey: [endpoint, JSON.stringify(params)],
  },
  onSuccess?: Function,
  onError?: Function,
  defaultHandlers: boolean = true,
): UseQueryResult<TResponse, any> => {
  const queryFn = async () => {
    try {
      const { data } = await api.get(endpoint, { params });
      if (defaultHandlers) onSuccessHandler(data);
      onSuccess?.(data, Number(params?.page) || 0);
      return data;
    } catch (error) {
      if (defaultHandlers) onErrorHandler(error);
      onError?.(error);
      throw error;
    }
  };
  return useQuery<TResponse, any>({
    queryFn,
    refetchOnWindowFocus: false,
    retry: false,
    ...options,
    queryKey: options.queryKey
      ? options.queryKey
      : [endpoint, JSON.stringify(params)],
  });
};

// Delete Data (DELETE)
export const useDeleteData = <TVariables, TResponse>(
  endpoint: string,
  Params?: any,
  options?: UseMutationOptions<TResponse, any, TVariables>,
  onSuccess?: Function,
  onError?: Function,
  defaultHandlers: boolean = true,
) => {
  return useMutation<TResponse, any, TVariables>({
    mutationFn: async ({ _id, params }: any) => {
      const { data } = await api.delete(`${endpoint}/${_id}`, {
        params: {
          ...Params,
          ...params,
        },
      });
      return data;
    },
    onSuccess: (response) => {
      if (defaultHandlers) onSuccessHandler(response);
      onSuccess?.(response);
    },
    onError: (error) => {
      if (defaultHandlers) onErrorHandler(error);
      onError?.(error);
    },
    ...options,
  });
};

const getRouteHandler = (method: routeMethod, endpoint: string) => {
  switch (method) {
    case routeMethods.Create:
      return (
        options?: any,
        onSuccess?: Function,
        onError?: Function,
        defautHanders?: boolean,
      ) => {
        return useCreateData(
          endpoint,
          options,
          onSuccess,
          onError,
          defautHanders,
        );
      };

    case routeMethods.Update:
      return (
        options?: any,
        onSuccess?: Function,
        onError?: Function,
        defautHanders?: boolean,
      ) => {
        return useUpdateData(
          endpoint,
          options,
          onSuccess,
          onError,
          defautHanders,
        );
      };

    case routeMethods.Delete:
      return (
        params?: any,
        options?: any,
        onSuccess?: Function,
        onError?: Function,
        defautHanders?: boolean,
      ) => {
        return useDeleteData(
          endpoint,
          params,
          options,
          onSuccess,
          onError,
          defautHanders,
        );
      };

    case routeMethods.Get:
      return (
        id: string,
        params?: any,
        options?: any,
        onSuccess?: Function,
        onError?: Function,
        defaultHanders?: boolean,
      ) => {
        return useGetData(
          `${endpoint}/${id}`,
          params,
          options,
          onSuccess,
          onError,
          defaultHanders,
        );
      };

    case routeMethods.GetList:
      return (
        params?: any,
        options?: any,
        onSuccess?: Function,
        onError?: Function,
        defaultHanders?: boolean,
      ) => {
        return useGetData(
          endpoint,
          params,
          options,
          onSuccess,
          onError,
          defaultHanders,
        );
      };
  }
};

const addHook = (
  hooks: Record<string, any>,
  routeName: string,
  route: string,
  method: string,
) => {
  const hookName = `use${method}`;
  if (hooks[routeName]) {
    hooks[routeName][hookName] = getRouteHandler(method as routeMethod, route);
  } else {
    hooks[routeName] = {
      [hookName]: getRouteHandler(method as routeMethod, route),
    };
  }
};

// Dynamically generate hooks for all endpoints
export const generateApiHooks = (routesConfig: RoutesType) => {
  const hooks = {};
    for (const [routeName, route] of Object.entries(routesConfig)) {
      if (typeof route === "string") {
        for (const method of routeMethodsArray) {
          addHook(hooks, routeName, route, method);
        }
      } else {
        for (const [method, curentRoute] of Object.entries(route)) {
          addHook(hooks, routeName, curentRoute, method);
        }
      }
  }
  return hooks;
};

